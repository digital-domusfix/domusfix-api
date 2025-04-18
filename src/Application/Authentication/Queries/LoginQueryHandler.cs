using DomusFix.Api.Application.Authentication.Queries;
using DomusFix.Api.Application.Common.Interfaces;
using DomusFix.Api.Application.Common.Models;
using DomusFix.Api.Domain.Entities.User;
using MediatR;

public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<TokenDto>>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public LoginQueryHandler(IIdentityService identityService, ITokenService tokenService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
    }

    public async Task<Result<TokenDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        // Validate user credentials
        var isValid = await _identityService.ValidateUserCredentialsAsync(request.Identifier, request.Password);
        if (!isValid)
        {
            return Result.Failure<TokenDto>("Invalid credentials.");
        }

        var user = await _identityService.FindByUserNameOrEmailAsync(request.Identifier);
        if (user == null)
        {
            return Result.Failure<TokenDto>("User not found.");
        }

        var token = _tokenService.GenerateToken(user);

        return Result.Success(new TokenDto
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email
            }
        });
    }
}
