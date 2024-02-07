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

            var user = User.Create("User1");
            await _fixture.InsertAsync(user);

            user.AddTopUpBeneficiary("Beneficiary1");

            var executeTopUpCommand = new ExecuteTopUpCommand(user.Id, new List<TopUpBeneficiaryCommand> { new TopUpBeneficiaryCommand(user.Beneficiaries.FirstOrDefault().Id, 5) });

            var result = await executeTopUpCommandHandler.Handle(executeTopUpCommand, CancellationToken.None);

            Assert.IsType<User>(result.Value);
        }
    }
}
