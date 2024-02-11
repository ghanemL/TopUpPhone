using ErrorOr;
using MobileTopup.Domain.UserAggregate.Entities;

namespace MobileTopup.Domain.UserAggregate
{
    public class User : IEntity
    {
        private const long maximumMontyhlyTopup = 3000;

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool IsVerified { get; set; } = true;

        public List<TopUpBeneficiary>? Beneficiaries { get; set; } = new List<TopUpBeneficiary>();

        private User(string name)
        {
            Name = name;
        }

        public User()
        {

        }

        public void AddTopUpBeneficiary(TopUpBeneficiary beneficiary)
        {
            Beneficiaries?.Add(beneficiary);
        }

        public void AddTransaction(TopUpBeneficiary beneficiary, long amount)
        {
            beneficiary.Transactions.Add(Transaction.Create(beneficiary, amount));
        }

        public static User Create(string name)
        {
            return new(name);
        }
        
        public decimal? GetMonthlyRemainingCapacity()
        {
            return maximumMontyhlyTopup - Beneficiaries?.Sum(b => b.GetCurrentTopUp());
        }

        public TopUpBeneficiary? GetTopUpBeneficiary(Guid beneficiaryId)
        {
            return Beneficiaries?.FirstOrDefault(b => b.Id == beneficiaryId);
        }

        public ErrorOr<bool> CheckBeneficiaryMonthlyTopUpCapacity(Guid beneficiaryId, long option)
        {
            var beneficiary = GetTopUpBeneficiary(beneficiaryId);

            if(beneficiary != null)
            {
                return beneficiary.CheckMonthlyTopUpCapacity(IsVerified, option);
            }

            return Error.NotFound("NotFound", $"Beneficiary not Found {beneficiaryId}");
        }
    }
}
