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

    public async Task<(Result Result, User? CreatedUser)> CreateUserAsync(string userName, string email, string password, string role = Roles.Customer, string? phone = null, string? address = null, string? postalCode = null)
    {
        var applicationUser = new ApplicationUser
        {
            UserName = userName,
            Email = email,
            PhoneNumber = phone,
            Address = address ?? string.Empty,
            PostalCode = postalCode ?? string.Empty
        };

        var result = await _userManager.CreateAsync(applicationUser, password);

        if (!result.Succeeded)
            return (result.ToApplicationResult(), null);

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
        if (user == null) return false;

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);
        return result.ToApplicationResult();
    }

    public async Task<Result> UpdateUserAsync(User user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id);
        if (appUser == null) return Result.Failure("User not found");

        appUser.UserName = user.UserName;
        appUser.PhoneNumber = user.Phone;
        appUser.Address = user.Address;
        appUser.PostalCode = user.PostalCode;
        appUser.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(appUser);
        return result.ToApplicationResult();
    }

    public async Task<User?> FindByUserNameOrEmailAsync(string identifier)
    {
        var appUser = await _userManager.Users
            .FirstOrDefaultAsync(u => u.UserName == identifier || u.Email == identifier);

        return appUser == null ? null : ToDomainUser(appUser);
    }

    public async Task<User> FindByIdAsync(string userId)
    {
        var appUser = await _userManager.FindByIdAsync(userId);
        return appUser == null ? throw new Exception("User not found") : ToDomainUser(appUser);
    }

    public async Task<bool> ValidateUserCredentialsAsync(string identifier, string password)
    {
        var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == identifier || u.Email == identifier);
        return appUser != null && await _userManager.CheckPasswordAsync(appUser, password);
    }

    public User ToDomainUser(ApplicationUser appUser) => new()
    {
        Id = appUser.Id,
        UserName = appUser.UserName ?? string.Empty,
        Name = appUser.Name ?? string.Empty,
        Email = appUser.Email ?? string.Empty,
        Phone = appUser.PhoneNumber ?? string.Empty,
        Address = appUser.Address ?? string.Empty,
        PostalCode = appUser.PostalCode ?? string.Empty,
        CreatedAt = appUser.CreatedAt,
        UpdatedAt = appUser.UpdatedAt
    };

    public ApplicationUser ToApplicationUser(User user) => new()
    {
        Id = user.Id,
        UserName = user.UserName,
        Name = user.Name,
        Email = user.Email,
        PhoneNumber = user.Phone,
        Address = user.Address,
        PostalCode = user.PostalCode,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt
    };
}
