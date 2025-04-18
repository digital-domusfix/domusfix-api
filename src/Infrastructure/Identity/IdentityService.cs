using DomusFix.Api.Application.Common.Interfaces;
using DomusFix.Api.Application.Common.Models;
using DomusFix.Api.Domain.Constants;
using DomusFix.Api.Domain.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DomusFix.Api.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user?.UserName;
    }

    public async Task<(Result Result, User? CreatedUser)> CreateUserAsync(string userName, string email, string password, string role = Roles.Customer)
    {
        var applicationUser = new ApplicationUser
        {
            UserName = userName,
            Email = email
        };

        var result = await _userManager.CreateAsync(applicationUser, password);

        if (!result.Succeeded)
            return (result.ToApplicationResult(), null);

        // Assign role after creation
        if (Roles.All.Contains(role))
            await _userManager.AddToRoleAsync(applicationUser, role);

        var domainUser = ToDomainUser(applicationUser);
        return (Result.Success(), domainUser);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }
    public async Task<Result> UpdateUserAsync(User user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id);
        if (appUser == null) return Result.Failure("User not found");

        appUser.UserName = user.UserName;
        appUser.Phone = user.Phone;
        appUser.Address = user.Address;
        appUser.PostalCode = user.PostalCode;
        appUser.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(appUser);
        return result.ToApplicationResult();
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }
    public async Task<User?> FindByUserNameOrEmailAsync(string identifier)
    {
        var applicationUser = await _userManager.Users
            .FirstOrDefaultAsync(u => u.UserName == identifier || u.Email == identifier);

        return applicationUser == null ? null : ToDomainUser(applicationUser);
    }

    public async Task<bool> ValidateUserCredentialsAsync(string identifier, string password)
    {
        var applicationUser = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.UserName == identifier || u.Email == identifier);

        if (applicationUser == null)
            return false;

        return await _userManager.CheckPasswordAsync(applicationUser, password);
    }

    // src/Infrastructure/Identity/IdentityService.cs
    public User ToDomainUser(ApplicationUser applicationUser)
    {
        return new User
        {
            Id = applicationUser.Id,
            UserName = applicationUser.UserName ?? "",
            Email = applicationUser.Email ?? "",
            Phone = applicationUser.Phone ?? "",
            Address = applicationUser.Address ?? "",
            PostalCode = applicationUser.PostalCode ?? ""
        };
    }

    public ApplicationUser ToApplicationUser(User domainUser)
    {
        return new ApplicationUser
        {
            Id = domainUser.Id,
            UserName = domainUser.UserName,
            Email = domainUser.Email,
            Phone = domainUser.Phone,
            Address = domainUser.Address,
            PostalCode = domainUser.PostalCode
        };
    }

    public async Task<User> FindByIdAsync(string userId)
    {
        var appUser = await _userManager.FindByIdAsync(userId);
        return appUser == null ? throw new Exception("User not found") : ToDomainUser(appUser);
    }

}
