using DomusFix.Api.Application.Common.Interfaces;

namespace DomusFix.Api.Application.Jobs.Queries;
public class GetMyJobsQueryHandler : IRequestHandler<GetMyJobsQuery, List<MyJobDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IUser _user;

    public GetMyJobsQueryHandler(IApplicationDbContext db, IUser user)
    {
        _db = db;
        _user = user;
    }

    public async Task<List<MyJobDto>> Handle(GetMyJobsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_user.Id))
            return [];

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var jobs = await _db.Jobs
            .Include(j => j.Quotes)
                .ThenInclude(q => q.Contractor)
            .Include(j => j.Contractor)
            .Include(j => j.AcceptedQuote)
                .ThenInclude(q =>   q.Contractor!)
            .Where(j => j.UserId == _user.Id)
            .OrderByDescending(j => j.CreatedAt)
            .Select(j => new MyJobDto
            {
                Id = j.Id,
                Title = j.Title,
                Type = j.SelectedService,
                PreferredDate = j.PreferredDate.ToString("yyyy-MM-dd") ?? j.CreatedAt.ToString("yyyy-MM-dd"),
                Description = j.Description,
                Status = j.Status,
                CreatedAt = j.CreatedAt,
                Contractor = j.Contractor != null ? new ContractorDto
                {
                    Id = j.Contractor.Id,
                    Name = j.Contractor.Name
                } : null,
                Quote = j.AcceptedQuote != null ? new QuoteDto
                {
                    Amount = j.AcceptedQuote.Amount,
                    Message = j.AcceptedQuote.Message,
                    Contractor = new ContractorDto
                    {
                        Id = j.AcceptedQuote.Contractor.Id,
                        Name = j.AcceptedQuote.Contractor.Name
                    }
                } : null
            })
            .ToListAsync(cancellationToken);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        return jobs;
    }
}
