using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomusFix.Api.Application.Common.Interfaces;
using DomusFix.Api.Application.Common.Models;

namespace DomusFix.Api.Application.Authentication.Commands;
public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result>
{
    private readonly IIdentityService _identityService;
    private readonly IUser _currentUser;

    public UpdateProfileCommandHandler(IIdentityService identityService, IUser currentUser)
    {
        _identityService = identityService;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        if (_currentUser.Id is null)
            return Result.Failure("User not authenticated.");

        var user = await _identityService.FindByIdAsync(_currentUser.Id);

        user.UserName = request.Name;
        user.Phone = request.Phone;
        user.Address = request.Address;
        user.PostalCode = request.PostalCode;
        user.UpdatedAt = DateTime.UtcNow;

        await _identityService.UpdateUserAsync(user);

        return Result.Success();
    }
}
