using ErrorOr;
using MediatR;
using MobileTopup.Application.Common.Interfaces.Persistance;
using MobileTopup.Domain.UserAggregate;
using MobileTopup.Domain.Common.Errors;
using MobileTopUp.Web.ExternalHttpClient;
using MobileTopup.Application.Common.Interfaces.Services;

namespace MobileTopup.Application.Topups.Commands.ExecuteTopup
{
    public class ExecuteTopUpCommandHandler : IRequestHandler<ExecuteTopUpCommand, ErrorOr<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly BalanceHttpClientService _balanceHttpService;


        public ExecuteTopUpCommandHandler(IUserRepository userRepository, 
            BalanceHttpClientService balanceHttpService)
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

            var checkUserGlobalCapacity = CheckMonthlyBeneficiaryCapacity(request, user);

            if (checkUserGlobalCapacity.Any(c => c.IsError))
            {
                return checkUserGlobalCapacity.FirstOrDefault().Errors;
            }

            long totalTopUpAmount = request.Beneficiaries.Sum(b => b.TopUpOption + 1);

            var remainingMonthlyCapacity = user.GetMonthlyRemainingCapacity();

            var balance = RetrieveBalance(user.Id, cancellationToken).Result;

            if (balance.IsError)
            {
                return balance.Errors;
            }

            if (balance.Value < totalTopUpAmount || remainingMonthlyCapacity < totalTopUpAmount)
            {
                return Errors.ExecuteTopUp.InsufficientBalance;
            }

            await Debit(user.Id, totalTopUpAmount, cancellationToken);

            ProcessTransaction(request, user);

            await _userRepository.SaveAsync(user);

            return user;
        }

        private async Task<ErrorOr<long>> RetrieveBalance(Guid userId, CancellationToken cancellationToken)
        {
            var response = await _balanceHttpService.GetBalanceAsync(userId.ToString(), cancellationToken);
            return long.TryParse(response, out long balance) ? balance : Errors.ExecuteTopUp.BalanceServiceUnavailable;
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

        public IEnumerable<ErrorOr<bool>> CheckMonthlyBeneficiaryCapacity(ExecuteTopUpCommand request, User user)
        {
            foreach(var beneficiary in request?.Beneficiaries)
            {
                yield return user.CheckBeneficiaryMonthlyTopUpCapacity(beneficiary.BeneficiaryId, beneficiary.TopUpOption);
            }
        }

        public void ProcessTransaction(ExecuteTopUpCommand request, User user)
        {
            Parallel.ForEach(request.Beneficiaries, (beneficiary) =>
            {
                var topUpBeneficiary = _userRepository.GetBeneficiary(user.Id, beneficiary.BeneficiaryId);
                topUpBeneficiary.AddTransaction(beneficiary.TopUpOption);
            });
        }
    }
}
