using MobileTopup.Application.Topups.Commands.ExecuteTopup;
using MobileTopup.Domain.UserAggregate;
using MobileTopup.IntegrationTests;
using Xunit;

namespace MobileTopup.Tests.Topups
{
    [Collection(nameof(SliceFixture))]
    public class ExecuteTopUpCommandTests
    {

        private readonly SliceFixture _fixture;

        public ExecuteTopUpCommandTests(SliceFixture fixture) => _fixture = fixture;


        [Fact]
        public async Task Handle_WithValidUserAndSufficientBalance_ShouldReturnUser()
        {
            var executeTopUpCommandHandler = new ExecuteTopUpCommandHandler(_fixture.UserRepository,
                _fixture.BalanceHttpService, 
                _fixture.DateTimeProvider);

            var user = User.Create($"User{Guid.NewGuid()}");
            user.AddTopUpBeneficiary("beneficiary1");
            await _fixture.InsertAsync(user);
            

            var executeTopUpCommand = new ExecuteTopUpCommand(user.Id, new List<TopUpBeneficiaryCommand> { new TopUpBeneficiaryCommand(user.Beneficiaries.First().Id, 5) });
            var result = await executeTopUpCommandHandler.Handle(executeTopUpCommand, CancellationToken.None);

            Assert.IsType<User>(result.Value);
        }

        [Fact]
        public async Task Handle_WithInvalidUser_ShouldReturnUserNotFound()
        {
            var executeTopUpCommandHandler = new ExecuteTopUpCommandHandler(_fixture.UserRepository,
                _fixture.BalanceHttpService,
                _fixture.DateTimeProvider);

            var executeTopUpCommand = new ExecuteTopUpCommand(Guid.NewGuid(), new List<TopUpBeneficiaryCommand> { new TopUpBeneficiaryCommand(Guid.NewGuid(), 5) });
            var result = await executeTopUpCommandHandler.Handle(executeTopUpCommand, CancellationToken.None);

            Assert.Equal(Domain.Common.Errors.Errors.User.UserNotFound.Code, result.Errors.First().Code);
        }

        [Fact]
        public async Task Handle_WithInsufficientBalance_ShouldReturnInsufficientBalanceError()
        {
            var executeTopUpCommandHandler = new ExecuteTopUpCommandHandler(_fixture.UserRepository,
                _fixture.BalanceHttpService,
                _fixture.DateTimeProvider);

            var user = User.Create($"User{Guid.NewGuid()}");
            user.AddTopUpBeneficiary("beneficiary1");
            await _fixture.InsertAsync(user);

            var executeTopUpCommand = new ExecuteTopUpCommand(user.Id, new List<TopUpBeneficiaryCommand> { new TopUpBeneficiaryCommand(user.Beneficiaries.First().Id, 500) });
            var result = await executeTopUpCommandHandler.Handle(executeTopUpCommand, CancellationToken.None);

            Assert.Equal(Domain.Common.Errors.Errors.ExecuteTopUp.InsufficientBalance.Code, result.Errors.First().Code);
        }

        [Fact]
        public async Task Handle_WithMonthlyCapacityExceeded_ShouldReturnMaximumCapacityError()
        {
            var executeTopUpCommandHandler = new ExecuteTopUpCommandHandler(_fixture.UserRepository,
                _fixture.BalanceHttpService,
                _fixture.DateTimeProvider);

            var user = User.Create($"User{Guid.NewGuid()}");
            user.AddTopUpBeneficiary("beneficiary1");
            await _fixture.InsertAsync(user);

            // Assuming Monthly Capacity is 3000 and the user already has a transaction of 2900
            user.AddTransaction(user.Beneficiaries.First(), 2900);
            await _fixture.InsertAsync(user);

            var executeTopUpCommand = new ExecuteTopUpCommand(user.Id, new List<TopUpBeneficiaryCommand> { new TopUpBeneficiaryCommand(user.Beneficiaries.First().Id, 200) });
            var result = await executeTopUpCommandHandler.Handle(executeTopUpCommand, CancellationToken.None);

            Assert.Equal(Domain.Common.Errors.Errors.ExecuteTopUp.MaximumCapacityExceedVerifiedUser.Code, result.Errors.First().Code);
        }

        [Fact]
        public async Task Handle_WithValidUserAndNoBeneficiaries_ShouldReturnUser()
        {
            var executeTopUpCommandHandler = new ExecuteTopUpCommandHandler(_fixture.UserRepository,
                _fixture.BalanceHttpService,
                _fixture.DateTimeProvider);

            var user = User.Create($"User{Guid.NewGuid()}");
            await _fixture.InsertAsync(user);

            var executeTopUpCommand = new ExecuteTopUpCommand(user.Id, new List<TopUpBeneficiaryCommand>());
            var result = await executeTopUpCommandHandler.Handle(executeTopUpCommand, CancellationToken.None);

            Assert.IsType<User>(result.Value);
        }
    }
}
