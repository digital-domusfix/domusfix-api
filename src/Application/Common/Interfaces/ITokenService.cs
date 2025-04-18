using DomusFix.Api.Domain.Entities.User;

namespace DomusFix.Api.Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
