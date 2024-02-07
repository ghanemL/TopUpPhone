
using ErrorOr;
using MediatR;
using MobileTopup.Contracts.Responses;

namespace MobileTopup.Application.Options.Queries
{
    public class GetAvailableTopUpOptionsQuery : IRequest<ErrorOr<TopUpOptionResponse>>
    {
    }
}
