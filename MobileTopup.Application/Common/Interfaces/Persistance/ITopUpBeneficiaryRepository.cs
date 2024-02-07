using MobileTopup.Domain.UserAggregate.Entities;

namespace MobileTopup.Application.Common.Interfaces.Persistance
{
    public interface ITopUpBeneficiaryRepository
    {
        TopUpBeneficiary GetById(Guid beneficiaryId);
        void Save(TopUpBeneficiary beneficiary);
    }
}
