using DomusFix.Api.Application.Authentication.Queries;
using DomusFix.Api.Application.Users.Commands.RegisterUser;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DomusFix.Api.Web.Endpoints;

public class Auth : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        var group = app.MapGroup(this);

        group.MapPost(RegisterUser, "register");
        group.MapPost(LoginUser, "login");
    }

    public async Task<Results<Ok<object>, BadRequest<string>>> RegisterUser(ISender sender, RegisterCommand command)
    {
        var result = await sender.Send(command);

        if (!result.IsSuccess)
            return TypedResults.BadRequest(string.Join(", ", result.Errors));

        return TypedResults.Ok((object)new { message = "User registered successfully" });
    }

    public async Task<Results<Ok<TokenDto>, UnauthorizedHttpResult>> LoginUser(ISender sender, LoginQuery query)
    {
        var result = await sender.Send(query);

        if (!result.IsSuccess)
            return TypedResults.Unauthorized();

        return TypedResults.Ok(result.Value);
    }
}
