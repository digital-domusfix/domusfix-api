using DomusFix.Api.Application.Common.Models;
using DomusFix.Api.Domain.Constants;

namespace DomusFix.Api.Application.Users.Commands.RegisterUser;
public class RegisterCommand : IRequest<Result>
{
    public string Name { get; set; } = string.Empty;  // 👈 new
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = Roles.Customer;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}




