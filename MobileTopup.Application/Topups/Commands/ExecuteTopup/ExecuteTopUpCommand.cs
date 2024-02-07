using MediatR;
using ErrorOr;
using MobileTopup.Domain.UserAggregate;

namespace MobileTopup.Application.Topups.Commands.ExecuteTopup
{
    public record ExecuteTopUpCommand
    (
        Guid UserId,
        List<TopUpBeneficiaryCommand>? Beneficiaries
    ) : IRequest<ErrorOr<User>>;


    public record TopUpBeneficiaryCommand
    (
        Guid BeneficiaryId,
        long TopUpOption);
}
