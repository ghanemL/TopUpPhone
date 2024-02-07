using MobileTopup.Domain.UserAggregate.Entities;

namespace MobileTopup.Domain.UserAggregate
{
    public class User
    {      
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public List<TopUpBeneficiary>? Beneficiaries { get; set; } = new List<TopUpBeneficiary>();

        private User(string name)
        {
            Name = name;
        }

        public User()
        {

        }

        public void AddTopUpBeneficiary(string nickname)
        {
            Beneficiaries.Add(TopUpBeneficiary.Create(this, nickname));
        }

        public void AddTransaction(TopUpBeneficiary beneficiary, long amount)
        {
            beneficiary.Transactions.Add(Transaction.Create(beneficiary, amount));
        }

        public static User Create(string name)
        {
            return new(name);
        }
    }
}
