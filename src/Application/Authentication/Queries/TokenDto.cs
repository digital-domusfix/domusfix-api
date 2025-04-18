namespace DomusFix.Api.Application.Authentication.Queries;
public class TokenDto
{
    public string Token { get; set; } = default!;
    public UserDto User { get; set; } = default!;
}
