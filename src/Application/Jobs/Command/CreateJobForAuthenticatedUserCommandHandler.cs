using DomusFix.Api.Application.Common.Interfaces;
using DomusFix.Api.Application.Common.Models;
using DomusFix.Api.Domain.Entities;
using DomusFix.Api.Domain.Entities.Jobs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DomusFix.Api.Application.Jobs.Command;

public class CreateJobForAuthenticatedUserCommandHandler : IRequestHandler<CreateJobForAuthenticatedUserCommand, Result<string>>
{
    private readonly IApplicationDbContext _db;
    private readonly IUser _user;

    public CreateJobForAuthenticatedUserCommandHandler(IApplicationDbContext db, IUser user)
    {
        _db = db;
        _user = user;
    }

    public async Task<Result<string>> Handle(CreateJobForAuthenticatedUserCommand request, CancellationToken cancellationToken)
    {
        if (_user.Id == null)
            return Result.Failure<string>("Authentication required.");

        var utcDate = DateTime.SpecifyKind(request.PreferredDate, DateTimeKind.Utc);

        // 👤 Create contact for this job
        var contact = new Contact
        {
            Name = request.Name ?? "Myself",
            Email = _user.Email ?? "",  // get from authenticated user
            Phone = request.Phone ?? "",
            Address = request.Address,
            PostalCode = request.PostalCode,
            UserId = _user.Id
        };

        _db.Contacts.Add(contact);

        // 🧾 Create job
        var job = new Job
        {
            Id = Guid.NewGuid().ToString(),
            UserId = _user.Id,
            Title = request.Title,
            SelectedService = request.SelectedService,
            Description = request.Description,
            PreferredDate = utcDate,
            Status = "in_progress",
            CreatedAt = DateTime.UtcNow,
            ContactId = contact.Id
        };

        _db.Jobs.Add(job);
        await _db.SaveChangesAsync(cancellationToken);

        return Result.Success(job.Id);
    }
}
