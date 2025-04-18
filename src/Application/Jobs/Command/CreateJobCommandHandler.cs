using DomusFix.Api.Application.Common.Interfaces;
using DomusFix.Api.Application.Common.Models;
using DomusFix.Api.Domain.Entities;
using DomusFix.Api.Domain.Entities.Jobs;

namespace DomusFix.Api.Application.Jobs.Command;

public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, Result<string>>
{
    private readonly IApplicationDbContext _db;
    private readonly IUser _currentUser;

    public CreateJobCommandHandler(IApplicationDbContext db, IUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Result<string>> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        var job = new Job
        {
            Id = Guid.NewGuid().ToString(),
            Title = request.Title,
            Description = request.Description,
            SelectedService = request.SelectedService,
            Status = "submitted",
            CreatedAt = DateTime.UtcNow,
        };

        // For guests or users — always associate a Contact
        var contact = new Contact
        {
            Name = request.Name ?? "Guest",
            Email = request.Email ?? "",
            Phone = request.Phone ?? "",
            Address = request.Address,
            PostalCode = request.PostalCode,
            UserId = _currentUser.Id // may be null if guest
        };

        _db.Contacts.Add(contact);
        job.ContactId = contact.Id;

        // Associate user if logged in
        if (_currentUser.Id != null)
        {
            job.UserId = _currentUser.Id;
        }

        _db.Jobs.Add(job);
        await _db.SaveChangesAsync(cancellationToken);

        return Result.Success(job.Id);
    }
}
