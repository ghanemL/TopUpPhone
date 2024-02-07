using ErrorOr;
using MediatR;
using MobileTopup.Contracts.Responses;
using MobileTopup.Domain.TopupOptions.Enums;

namespace MobileTopup.Application.Options.Queries
{
    public class GetAvailableTopUpOptionsQueryHandler : IRequestHandler<GetAvailableTopUpOptionsQuery, ErrorOr<TopUpOptionResponse>>
    {
        public async Task<ErrorOr<TopUpOptionResponse>> Handle(GetAvailableTopUpOptionsQuery request, CancellationToken cancellationToken)
        {
            return new TopUpOptionResponse { Options = Enum.GetNames(typeof(TopUpOption)).ToList() };
        }
    }
}
