using DomusFix.Api.Application.Authentication.Queries;
using DomusFix.Api.Application.Common.Interfaces;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserDto>
{
    private readonly IIdentityService _identityService;
    private readonly IUser _currentUser;

    public GetProfileQueryHandler(IIdentityService identityService, IUser currentUser)
    {
        _identityService = identityService;
        _currentUser = currentUser;
    }

    public async Task<UserDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        if (_currentUser.Id is null)
            throw new UnauthorizedAccessException();

        var user = await _identityService.FindByIdAsync(_currentUser.Id);

        return new UserDto
        {
            Id = user.Id,
            Name = user.UserName,
            Email = user.Email,
            Phone = user.Phone ?? "",
            Address = user.Address ?? "",
            PostalCode = user.PostalCode ?? ""
        };
    }
}
