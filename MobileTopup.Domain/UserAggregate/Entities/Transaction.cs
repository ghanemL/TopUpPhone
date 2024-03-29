﻿
namespace MobileTopup.Domain.UserAggregate.Entities
{
    public class Transaction : IEntity
    {
        private readonly long FEES = 1;
        public Guid Id { get; set; }
        public long Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public TopUpBeneficiary? TopUpBeneficiary { get; set; }
        public Guid? TopUpBeneficiaryId { get; set; }

        public Transaction(Guid guid, TopUpBeneficiary beneficiary, long option, DateTime now)
        {
            Id = guid;
            Amount = option + FEES;
            TransactionDate = now;
            TopUpBeneficiary = beneficiary;
            TopUpBeneficiaryId = beneficiary.Id;
        }  
        
        public Transaction()
        {

        }

        public static Transaction Create(TopUpBeneficiary beneficiary, long option)
        {
            return new Transaction(Guid.NewGuid(), beneficiary, option, DateTime.Now);
        }


        public bool IsInCurrentMonth()
        {
            return (TransactionDate.Year == DateTime.UtcNow.Year && TransactionDate.Month == DateTime.UtcNow.Month);
        }
    }
}
