using DomusFix.Api.Application.Common.Models;
using DomusFix.Api.Domain.Entities.User;

namespace DomusFix.Api.Application.Authentication.Queries;
public class LoginQuery : IRequest<Result<TokenDto>>
{
    public string Identifier { get; set; } = string.Empty; // Can be email or username
    public string Password { get; set; } = string.Empty;
}

