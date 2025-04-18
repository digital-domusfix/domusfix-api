using DomusFix.Api.Application.Common.Interfaces;
using DomusFix.Api.Application.Common.Models;
using DomusFix.Api.Application.Users.Commands.RegisterUser;
using DomusFix.Api.Domain.Entities;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
{
    private readonly IIdentityService _identityService;
    private readonly IApplicationDbContext _db;

    public RegisterCommandHandler(IIdentityService identityService, IApplicationDbContext db)
    {
        _identityService = identityService;
        _db = db;
    }

    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var (result, user) = await _identityService.CreateUserAsync(
            request.Name, request.Email, request.Password, request.Role);

        if (!result.IsSuccess || user == null)
            return Result.Failure(result.Errors);

        // 🧾 Create contact entry linked to this user
        var contact = new Contact
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            PostalCode = request.PostalCode,
            UserId = user.Id
        };

        _db.Contacts.Add(contact);
        await _db.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

