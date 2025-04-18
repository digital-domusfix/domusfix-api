using DomusFix.Api.Application.Common.Models;
using DomusFix.Api.Domain.Constants;
using DomusFix.Api.Domain.Entities.User;

namespace DomusFix.Api.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, User? CreatedUser)> CreateUserAsync(string userName, string email, string password, string role = Roles.Customer);

    Task<bool> ValidateUserCredentialsAsync(string identifier, string password);

    Task<User?> FindByUserNameOrEmailAsync(string identifier);

    Task<Result> DeleteUserAsync(string userId);
    Task<User> FindByIdAsync(string userId); 
    Task<Result> UpdateUserAsync(User user);


}
