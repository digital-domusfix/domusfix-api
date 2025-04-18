using DomusFix.Api.Application.Common.Interfaces;

namespace DomusFix.Api.Application.Jobs.Queries;
public class GetJobByIdQueryHandler : IRequestHandler<GetJobByIdQuery, JobDto?>
{
    private readonly IApplicationDbContext _db;
    private readonly IUser _user;

    public GetJobByIdQueryHandler(IApplicationDbContext db, IUser user)
    {
        _db = db;
        _user = user;
    }

    public async Task<JobDto?> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
    {
        var job = await _db.Jobs.FindAsync(new object[] { request.JobId }, cancellationToken);

        if (job == null)
            return null;

        // Optional: Restrict access for authenticated users to their own job
        if (!string.IsNullOrEmpty(job.UserId) && _user.Id != job.UserId)
            return null;

        return new JobDto
        {
            Id = job.Id,
            SelectedService = job.SelectedService,
            Title = job.Title,
            Description = job.Description,
            Status = job.Status,
            CreatedAt = job.CreatedAt
        };
    }
}
