using ErrorOr;
using MediatR;
using MobileTopup.Application.Common.Interfaces.Persistance;
using MobileTopup.Domain.UserAggregate;
using MobileTopup.Domain.Common.Errors;
using MobileTopUp.Web.ExternalHttpClient;
using MobileTopup.Application.Common.Interfaces.Services;
using MobileTopup.Domain.UserAggregate.Entities;

namespace MobileTopup.Application.Topups.Commands.ExecuteTopup
{
    public class ExecuteTopUpCommandHandler : IRequestHandler<ExecuteTopUpCommand, ErrorOr<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpClientService _balanceHttpService;
        private readonly IDateTimeProvider _dateTimeProvider;

        private const int MaximumTopUpExceedVerified = 500;
        private const int MaximumTopUpExceedUnverified = 100;
        private const int MonthlyCapacityLimit = 3000;


        public ExecuteTopUpCommandHandler(IUserRepository userRepository, 
            IHttpClientService balanceHttpService,
            IDateTimeProvider dateTimeProvider)
        {
            _userRepository = userRepository;
            _balanceHttpService = balanceHttpService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<User>> Handle(ExecuteTopUpCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetById(request.UserId);

            if (user == null)
            {
                return Errors.User.UserNotFound;
            }

            var balance = RetrieveBalance(user.Id, cancellationToken).Result.Value;

            var errors = CheckMonthlyBeneficiaryCapacity(request, user);

            if (errors.Any())
            {
                return errors.First();
            }

            long totalTopUpAmount = request.Beneficiaries.Sum(b => b.TopUpOption + 1);

            var remainingMonthlyCapacity = MonthlyCapacityLimit - user.Beneficiaries?.Sum(b => GetUserBeneficiaryCurrentTopup(b));

            if (balance < totalTopUpAmount || remainingMonthlyCapacity < totalTopUpAmount)
            {
                return Errors.ExecuteTopUp.InsufficientBalance;
            }

            await Debit(user.Id, totalTopUpAmount, cancellationToken);

            ProcessTransaction(request, user);

            await _userRepository.SaveAsync(user);

            return user;
        }

        private async Task<ErrorOr<long?>> RetrieveBalance(Guid userId, CancellationToken cancellationToken)
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

        public List<Error> CheckMonthlyBeneficiaryCapacity(ExecuteTopUpCommand request, User user)
        {
            var errors = new List<Error>();

            foreach (var beneficiary in request.Beneficiaries)
            {
                var userBeneficiary = user.Beneficiaries.SingleOrDefault(ub => ub.Id == beneficiary.BeneficiaryId);

                if (userBeneficiary != null)
                {
                    var userBeneficiaryCurrentTopup = GetUserBeneficiaryCurrentTopup(userBeneficiary);

                    if (userBeneficiaryCurrentTopup + beneficiary.TopUpOption > MaximumTopUpExceedVerified && user.IsVerified)
                    {
                        errors.Add(Errors.ExecuteTopUp.MaximumCapacityExceedVerifiedUser);
                    }

                    if (userBeneficiaryCurrentTopup + beneficiary.TopUpOption > MaximumTopUpExceedUnverified && !user.IsVerified)
                    {
                        errors.Add(Errors.ExecuteTopUp.MaximumCapacityExceedUnverifiedUser);
                    }
                }
            }

            return errors;
        }

        public void ProcessTransaction(ExecuteTopUpCommand request, User user)
        {
            request.Beneficiaries.ForEach(beneficiary =>
            {
                var currentBeneficiary = user?.Beneficiaries?.SingleOrDefault(b => b.Id == beneficiary.BeneficiaryId);
                user?.AddTransaction(currentBeneficiary, beneficiary.TopUpOption + 1);
            });
        }

        private decimal GetUserBeneficiaryCurrentTopup(TopUpBeneficiary userBeneficiary)
        {
            return userBeneficiary.Transactions
                ?.Where(t => _dateTimeProvider.IsInCurrentMonth(t.TransactionDate))
                .Sum(t => t?.Amount) ?? 0;
        }
    }
}
