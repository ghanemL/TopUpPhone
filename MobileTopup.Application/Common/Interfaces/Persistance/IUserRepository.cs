using MobileTopup.Domain.UserAggregate;
using MobileTopup.Domain.UserAggregate.Entities;

namespace MobileTopup.Application.Common.Interfaces.Persistance
{    public interface IUserRepository
    {
        User GetById(Guid userId);
        User GetByName(string name);
        Task SaveAsync(User user);
        Task SaveChangesAsync();
        TopUpBeneficiary GetBeneficiary(Guid id, Guid beneficiaryId);
    }
}
