using MobileTopup.Application.Topups.Commands.AddTopupBeneficiary;
using MobileTopup.IntegrationTests;
using Xunit;
using Shouldly;

namespace MobileTopup.Tests.Topups
{
    [Collection(nameof(SliceFixture))]
    public class AddTopUpBeneficiaryCommandTests
    {
        private readonly SliceFixture _fixture;

        public AddTopUpBeneficiaryCommandTests(SliceFixture fixture) => _fixture = fixture;

        [Fact]
        public void AddTopUpBeneficiaryCommand_ValidParameters_ShouldPassValidation()
        {
            var command = new AddTopUpBeneficiaryCommand(Guid.NewGuid(), "Beneficiary1");

            var validator = new AddTopUpBeneficiaryCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void AddTopUpBeneficiaryCommand_InvalidUserId_ShouldFailValidation()
        {

            var command = new AddTopUpBeneficiaryCommand(Guid.Empty, "Beneficiary1");

            var validator = new AddTopUpBeneficiaryCommandValidator();
            var result = validator.Validate(command);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.PropertyName == nameof(AddTopUpBeneficiaryCommand.UserId));
        }

        [Fact]
        public void AddTopUpBeneficiaryCommand_ExeedNickNameLenght_ShouldFailValidation()
        {

            var command = new AddTopUpBeneficiaryCommand(Guid.NewGuid(), "Beneficiary1Beneficiary1");

            var validator = new AddTopUpBeneficiaryCommandValidator();
            var result = validator.Validate(command);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.PropertyName == nameof(AddTopUpBeneficiaryCommand.Nickname));
        }
    }
}
