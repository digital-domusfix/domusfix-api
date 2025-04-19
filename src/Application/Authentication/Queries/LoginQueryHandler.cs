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
        var isValid = await _identityService.ValidateUserCredentialsAsync(request.Identifier, request.Password);
        if (!isValid)
            return Result.Failure<TokenDto>("Invalid credentials.");

        var user = await _identityService.FindByUserNameOrEmailAsync(request.Identifier);
        if (user == null)
            return Result.Failure<TokenDto>("User not found.");

        var token = _tokenService.GenerateToken(user);

        var dto = new TokenDto
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                PostalCode = user.PostalCode
            }
        };

        return Result.Success(dto);
    }

}
