using ErrorOr;
using MediatR;
using MobileTopup.Application.Common.Interfaces.Persistance;
using MobileTopup.Domain.UserAggregate;
using MobileTopup.Domain.Common.Errors;
using MobileTopUp.Web.ExternalHttpClient;

namespace MobileTopup.Application.Topups.Commands.ExecuteTopup
{
    public class ExecuteTopUpCommandHandler : IRequestHandler<ExecuteTopUpCommand, ErrorOr<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpClientService _balanceHttpService;

        public ExecuteTopUpCommandHandler(IUserRepository userRepository, IHttpClientService balanceHttpService)
        {
            _userRepository = userRepository;
            _balanceHttpService = balanceHttpService;
        }

        public async Task<ErrorOr<User>> Handle(ExecuteTopUpCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetById(request.UserId);

            if (user == null)
            {
                return Errors.User.UserNotFound;
            }

            long totalTopUpAmount = request.Beneficiaries.Sum(b => b.TopUpOption + 1);

            var balance = RetrieveBalance(user.UserId, cancellationToken).Result.Value;

            if (balance < totalTopUpAmount || balance < 3000)
            {
                return Errors.ExecuteTopUp.InsufficientBalance;
            }

            await Debit(user.UserId, totalTopUpAmount, cancellationToken);

            request.Beneficiaries.ForEach(beneficiary => {
                var currentBeneficiary = user?.Beneficiaries?.SingleOrDefault(b => b.BeneficiaryId == beneficiary.BeneficiaryId);
                user.AddTransaction(currentBeneficiary, beneficiary.TopUpOption + 1);
            });           
            
            await _userRepository.SaveAsync(user);

            return user;
        }

        public async Task<ErrorOr<long?>> RetrieveBalance(Guid userId, CancellationToken cancellationToken)
        {
            var response = await _balanceHttpService.GetBalanceAsync(userId.ToString(), cancellationToken);
            if (response == null)
            {
                return Errors.ExecuteTopUp.BalanceServiceUnavailable;
            }
            long balance;
            long.TryParse(response, out balance);
            return balance;
        }

        public async Task<ErrorOr<bool>> Debit(Guid userId, long amount, CancellationToken cancellationToken)
        {
            var response = await _balanceHttpService.DebitAsync(userId.ToString(), amount, cancellationToken);
            if (!response)
            {
                return Errors.ExecuteTopUp.BalanceServiceUnavailable;
            }
            
            return response;
        }
    }
}
