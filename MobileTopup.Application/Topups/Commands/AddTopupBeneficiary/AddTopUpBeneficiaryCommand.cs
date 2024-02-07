using ErrorOr;
using MediatR;
using MobileTopup.Domain.UserAggregate;

namespace MobileTopup.Application.Topups.Commands.AddTopupBeneficiary
{
    public record AddTopUpBeneficiaryCommand
    (
        Guid UserId,
        string? Nickname) : IRequest<ErrorOr<User>>;
        
}
