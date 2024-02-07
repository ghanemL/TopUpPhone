
using FluentResults;
using MediatR;
using MobileTopup.Contracts.Responses;

namespace MobileTopup.Application.Topups.Queries.GetTopUpBeneficiaries
{
    public record GetTopUpBeneficiariesQuery : IRequest<Result<List<TopUpBeneficiaryResponse>>>
    {
        public Guid UserId { get; set; }
    }
}
