
using ErrorOr;

namespace MobileTopup.Domain.UserAggregate.Entities
{
    public class TopUpBeneficiary
    {
        public const long MaximumVerifiedMonthlyBenificiaryCapacity = 300;
        public const long MaximumUnVerifiedMonthlyBenificiaryCapacity = 100;

        public Guid Id { get; set; }
        public string? Nickname { get; set; }
        public User? User { get; set; }
        public Guid? UserId { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public TopUpBeneficiary(Guid guid, User user, string? nickName)
        {
            Id = guid;
            UserId = user.Id;
            User = user;
            Nickname = nickName;
        }      

        public TopUpBeneficiary()
        {

        }

        public static TopUpBeneficiary Create(User user, string nickName)
        {
            return new TopUpBeneficiary(Guid.NewGuid(), user, nickName);
        }
        public void AddTransaction(long topUpOption)
        {
            Transactions.Add(Transaction.Create(this, topUpOption));
        }

        public decimal GetCurrentTopUp()
        {
            return Transactions
                ?.Where(t => t.IsInCurrentMonth())
                .Sum(t => t?.Amount) ?? 0;
        }

        public ErrorOr<bool> CheckMonthlyTopUpCapacity(bool isVerified, long option)
        {
            var currentTopUp = GetCurrentTopUp();

            if (currentTopUp + option + 1 > (isVerified ? MaximumVerifiedMonthlyBenificiaryCapacity : MaximumUnVerifiedMonthlyBenificiaryCapacity ))
            {
                return Error.Custom(480, "Benefciary.Id", $"Maximum TopUp capacity for beneficiary {Id} Exceed by Month");
            }
            return true;
        }
    }    
}
