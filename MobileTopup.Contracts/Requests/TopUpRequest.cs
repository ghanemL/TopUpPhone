using MobileTopup.Domain.TopupOptions.Enums;

namespace MobileTopup.Contracts.Requests
{
    public class TopUpRequest
    {
        public Guid BeneficiaryId { get; set; }
        public TopUpOption TopUpOption { get; set; }
    }
}
