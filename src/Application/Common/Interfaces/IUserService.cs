using DomusFix.Api.Domain.Entities.User;

namespace Domusfix.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetProvidersAsync();
    }
}
