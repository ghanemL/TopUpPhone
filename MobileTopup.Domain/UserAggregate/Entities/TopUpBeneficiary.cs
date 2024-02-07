
namespace MobileTopup.Domain.UserAggregate.Entities
{
    public class TopUpBeneficiary
    {
        public Guid BeneficiaryId { get; set; }
        public string? Nickname { get; set; }
        public User? User { get; set; }
        public Guid? UserId { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public TopUpBeneficiary(Guid guid, User user, string? nickName)
        {
            BeneficiaryId = guid;
            UserId = user.UserId;
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
        public void AddTransaction(Guid beneficiaryId, long topUpOption)
        {
            Transactions.Add(Transaction.Create(this, topUpOption));
        }
    }

    
}
