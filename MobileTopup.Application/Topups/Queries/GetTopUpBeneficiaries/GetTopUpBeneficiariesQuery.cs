
using ErrorOr;
using FluentResults;
using MediatR;
using MobileTopup.Contracts.Responses;

namespace MobileTopup.Application.Topups.Queries.GetTopUpBeneficiaries
{
    public record GetTopUpBeneficiariesQuery : IRequest<ErrorOr<List<TopUpBeneficiaryResponse>>>
    {
        public Guid UserId { get; set; }
    }
}
