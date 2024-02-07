namespace MobileTopup.Contracts.Responses
{
    public record UserResponse(
        Guid Id, 
        string Name,
        List<TopupBeneficiaryResponse> Beneficiaries);

    public record TopupBeneficiaryResponse(
        Guid Id,
        string Nickname,
        List<TransactionResponse> Transactions);

    public record TransactionResponse(
        Guid Id, 
        long TopUpOption,
        DateTime TransactionDate);


}
