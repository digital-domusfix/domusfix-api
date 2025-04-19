using DomusFix.Api.Application.Authentication.Commands;
using DomusFix.Api.Application.Authentication.Queries;
using DomusFix.Api.Application.Jobs.Queries;
using DomusFix.Api.Infrastructure.Identity;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DomusFix.Api.Web.Endpoints;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        var group = app.MapGroup(this)
            .RequireAuthorization();

        group.MapIdentityApi<ApplicationUser>();

        group.MapGet(GetCurrentUser, "me");
        group.MapPut(UpdateProfile, "me");
        group.MapGet(GetMyJobs, "me/jobs"); // 🆕 Add the job listing endpoint
    }

    public async Task<Results<Ok<UserDto>, UnauthorizedHttpResult>> GetCurrentUser(ISender sender)
    {
        var user = await sender.Send(new GetProfileQuery());
        return TypedResults.Ok(user);
    }

    public async Task<Results<Ok<string>, BadRequest<string>>> UpdateProfile(
        ISender sender, UpdateProfileCommand command)
    {
        var result = await sender.Send(command);

        if (!result.IsSuccess)
            return TypedResults.BadRequest(string.Join(", ", result.Errors));

        return TypedResults.Ok("Profile updated successfully.");
    }

    public async Task<Results<Ok<List<MyJobDto>>, UnauthorizedHttpResult>> GetMyJobs(ISender sender)
    {
        var jobs = await sender.Send(new GetMyJobsQuery());
        return TypedResults.Ok(jobs);
    }
}
