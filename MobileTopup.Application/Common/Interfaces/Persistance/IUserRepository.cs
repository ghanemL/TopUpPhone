using MobileTopup.Domain.UserAggregate;

namespace MobileTopup.Application.Common.Interfaces.Persistance
{    public interface IUserRepository
    {
        User GetById(Guid userId);
        User GetByName(string name);
        Task SaveAsync(User user);
        Task SaveChangesAsync();
    }
}
