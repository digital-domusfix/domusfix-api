using DomusFix.Api.Application.Jobs.Command;
using DomusFix.Api.Application.Jobs.Queries;

public class Jobs : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        var group = app.MapGroup(this);

        group.MapPost("", async (ISender sender, CreateJobCommand command) =>
        {
            var result = await sender.Send(command);

            if (!result.IsSuccess)
                return Results.BadRequest(string.Join(", ", result.Errors));

            return Results.Ok(new
            {
                jobId = result.Value,
                status = "in_progress"
            });
        })
        .AllowAnonymous();

        group.MapPost("/authenticated", CreateJobForAuthenticatedUser)
              .RequireAuthorization();// ✅ Works without error
        group.MapGet("/{id}", GetJobById)
              .RequireAuthorization();
    }

    public async Task<IResult> CreateJobForAuthenticatedUser(
    ISender sender, CreateJobForAuthenticatedUserCommand command)
    {
        var result = await sender.Send(command);

        if (!result.IsSuccess)
            return TypedResults.BadRequest(string.Join(", ", result.Errors));

        return TypedResults.Ok(new
        {
            jobId = result.Value,
            status = "in_progress"
        });
    }
    public async Task<IResult> GetJobById(string id, ISender sender)
    {
        var job = await sender.Send(new GetJobByIdQuery { JobId = id });

        if (job == null)
            return Results.NotFound();

        return Results.Ok(job);
    }



}
