
using FluentResults;
using MediatR;
using MobileTopup.Application.Common.Interfaces.Persistance;
using MobileTopup.Contracts.Responses;

namespace MobileTopup.Application.Topups.Queries.GetTopUpBeneficiaries
{
    public class GetTopUpBeneficiariesQueryHandler : IRequestHandler<GetTopUpBeneficiariesQuery, Result<List<TopUpBeneficiaryResponse>>>
    {
        private readonly IUserRepository _userRepository;

        public GetTopUpBeneficiariesQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<List<TopUpBeneficiaryResponse>>> Handle(GetTopUpBeneficiariesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _userRepository.GetById(request.UserId);

                if (user == null)
                {
                    return Result.Fail("User not found.");
                }

                var beneficiaries = user.Beneficiaries
                    .Select(b => new TopUpBeneficiaryResponse
                    {
                        BeneficiaryId = b.BeneficiaryId,
                        Nickname = b.Nickname
                    })
                    .ToList();

                return Result.Ok(beneficiaries);
            }
            catch (Exception ex)
            {
                return Result.Fail("An error occurred while processing the request.");
            }
        }
    }
}
