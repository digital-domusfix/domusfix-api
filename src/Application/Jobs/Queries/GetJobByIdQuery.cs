namespace DomusFix.Api.Application.Jobs.Queries;


public class GetJobByIdQuery : IRequest<JobDto>
{
    public string JobId { get; set; } = default!;
}

