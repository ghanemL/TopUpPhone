
using ErrorOr;
using MediatR;
using MobileTopup.Application.Common.Interfaces.Persistance;
using MobileTopup.Domain.UserAggregate;
using MobileTopup.Domain.Common.Errors;

namespace MobileTopup.Application.Topups.Commands.AddTopupBeneficiary
{
    public class AddTopUpBeneficiaryCommandHandler : IRequestHandler<AddTopUpBeneficiaryCommand, ErrorOr<User>>
    {
        private readonly IUserRepository _userRepository;

        public AddTopUpBeneficiaryCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<User>> Handle(AddTopUpBeneficiaryCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetById(request.UserId);

            if (user == null)
            {
                return Errors.User.UserNotFound;
            }

            if (user.Beneficiaries?.Count >= 5)
            {
                return Errors.User.UserMaximumBeneficiaryAttempt;
            }

            if (user.Beneficiaries.Exists(b => string.Equals(b.Nickname, request.Nickname, StringComparison.CurrentCultureIgnoreCase)))
            {
                return Errors.User.BeneficiaryAlreadyExists;
            }

            user.AddTopUpBeneficiary(request.Nickname);

            await _userRepository.SaveChangesAsync();

            return user;
        }
    }
}
