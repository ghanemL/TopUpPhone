namespace MobileTopup.Contracts.Requests
{
    public class ExecuteTopUpRequest
    {
        public Guid UserId { get; set; }
        public List<TopUpRequest>? TopUpRequests { get; set; }
    }
}
