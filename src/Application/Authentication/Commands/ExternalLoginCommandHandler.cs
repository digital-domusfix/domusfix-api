using System.IdentityModel.Tokens.Jwt;
using DomusFix.Api.Application.Authentication.Commands;
using DomusFix.Api.Application.Authentication.Queries;
using DomusFix.Api.Application.Common.Interfaces;
using Google.Apis.Auth;

public class ExternalLoginCommandHandler : IRequestHandler<ExternalLoginCommand, TokenDto>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public ExternalLoginCommandHandler(IIdentityService identityService, ITokenService tokenService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
    }

    public async Task<TokenDto> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
    {
        string? email = null;
        string? name = null;

        if (request.Provider == "google")
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
            email = payload.Email;
            name = payload.Name;
        }
        else if (request.Provider == "apple")
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(request.IdToken);
            email = jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            name = jwt.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "Apple User";
        }
        else
        {
            throw new Exception("Unsupported provider");
        }

        if (email is null) throw new Exception("Invalid ID token");

        var user = await _identityService.FindByUserNameOrEmailAsync(email);

        if (user is null)
        {
            var (result, createdUser) = await _identityService.CreateUserAsync(name ?? email, email, Guid.NewGuid().ToString()); // random pw
            if (!result.IsSuccess) throw new Exception("Failed to create user");

            user = createdUser!;
        }

        var token = _tokenService.GenerateToken(user);
        return new TokenDto
        {
            Token = _tokenService.GenerateToken(user),
            User = new UserDto
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email
            }
        };

    }
}
