
using ErrorOr;
using MediatR;
using MobileTopup.Application.Common.Interfaces.Persistance;
using MobileTopup.Contracts.Responses;

namespace MobileTopup.Application.Topups.Queries.GetTopUpBeneficiaries
{
    public class GetTopUpBeneficiariesQueryHandler : IRequestHandler<GetTopUpBeneficiariesQuery, ErrorOr<List<TopUpBeneficiaryResponse>>>
    {
        private readonly IUserRepository _userRepository;

        public GetTopUpBeneficiariesQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<List<TopUpBeneficiaryResponse>>> Handle(GetTopUpBeneficiariesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _userRepository.GetById(request.UserId);

                if (user == null)
                {
                    return ErrorOr.Error.NotFound("User not found.");
                }

                var beneficiaries = user.Beneficiaries
                    .Select(b => new TopUpBeneficiaryResponse
                    {
                        BeneficiaryId = b.Id,
                        Nickname = b.Nickname
                    })
                    .ToList();

                return beneficiaries;
            }
            catch (Exception ex)
            {
                return ErrorOr.Error.Failure("An error occurred while processing the request.");
            }
        }
    }
}
