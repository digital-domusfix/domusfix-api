using DomusFix.Api.Application.Authentication.Commands;
using DomusFix.Api.Application.Authentication.Queries;
using Microsoft.AspNetCore.Http.HttpResults;

public class ExternalAuth : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(LoginWithGoogle, "auth/login-google")
            .MapPost(LoginWithApple, "auth/login-apple");
    }

    public async Task<Results<Ok<TokenDto>, BadRequest<string>>> LoginWithGoogle(ISender sender, ExternalLoginCommand command)
    {
        if (command.Provider != "google") return TypedResults.BadRequest("Invalid provider.");
        var token = await sender.Send(command);
        return TypedResults.Ok(token);
    }

    public async Task<Results<Ok<TokenDto>, BadRequest<string>>> LoginWithApple(ISender sender, ExternalLoginCommand command)
    {
        if (command.Provider != "apple") return TypedResults.BadRequest("Invalid provider.");
        var token = await sender.Send(command);
        return TypedResults.Ok(token);
    }
}
