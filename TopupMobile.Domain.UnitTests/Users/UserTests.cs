using ErrorOr;
using FluentAssertions;
using MobileTopup.Domain.UserAggregate;
using MobileTopup.Domain.UserAggregate.Entities;
using Xunit;

namespace TopupMobile.Domain.UnitTests.Users
{
    public class UserTests
    {
        [Fact]
        public void SetUser_ShouldSuccess()
        {
            var user = User.Create("User1");

            user.Should().NotBeNull();
        }

        [Fact]
        public void AddTopUpBeneficiary_ShouldAddBeneficiary()
        {
            var user = User.Create("User2");
            var beneficiary = new TopUpBeneficiary();

            user.AddTopUpBeneficiary(beneficiary);

            user.Beneficiaries.Should().Contain(beneficiary);
        }

        [Fact]
        public void AddTransaction_ShouldAddTransaction()
        {
            var user = User.Create("User3");
            var beneficiary = new TopUpBeneficiary();
            user.AddTopUpBeneficiary(beneficiary);

            user.AddTransaction(beneficiary, 100);

            beneficiary.Transactions.Should().HaveCount(1);
        }

        [Fact]
        public void GetMonthlyRemainingCapacity_ShouldCalculateCorrectly()
        {
            var user = User.Create("User1");
            var beneficiary = new TopUpBeneficiary();
            beneficiary.AddTransaction(100);
            user.AddTopUpBeneficiary(beneficiary);

            var remainingCapacity = user.GetMonthlyRemainingCapacity();

            remainingCapacity.Should().Be(2899); // Assuming maximumMontyhlyTopup is 3000
        }

        [Fact]
        public void GetTopUpBeneficiary_ShouldReturnBeneficiary()
        {
            var user = User.Create("User1");
            var beneficiary = new TopUpBeneficiary();
            user.AddTopUpBeneficiary(beneficiary);

            var retrievedBeneficiary = user.GetTopUpBeneficiary(beneficiary.Id);

            retrievedBeneficiary.Should().Be(beneficiary);
        }

        [Fact]
        public void CheckBeneficiaryMonthlyTopUpCapacity_ShouldReturnSuccess()
        {
            var user = User.Create("User1");
            var beneficiary = new TopUpBeneficiary();
            user.AddTopUpBeneficiary(beneficiary);

            var result = user.CheckBeneficiaryMonthlyTopUpCapacity(beneficiary.Id, 10);
            result.IsError.Should().Be(false);

        }

        [Fact]
        public void CheckBeneficiaryMonthlyTopUpCapacity_ShouldReturnNotFound()
        {
            var user = User.Create("User1");

            var result = user.CheckBeneficiaryMonthlyTopUpCapacity(Guid.NewGuid(), 200);

            result.IsError.Should().BeTrue();
            result.Errors.First().Code.Should().Be(ErrorType.NotFound.ToString());
        }

    }


}
