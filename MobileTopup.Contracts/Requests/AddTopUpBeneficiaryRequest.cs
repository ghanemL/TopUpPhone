
namespace MobileTopup.Contracts.Requests
{
    public class AddTopUpBeneficiaryRequest
    {
        public Guid UserId { get; set; }
        public string? Nickname { get; set; }
    }
}
