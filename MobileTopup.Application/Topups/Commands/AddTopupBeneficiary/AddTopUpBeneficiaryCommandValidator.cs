
using FluentValidation;

namespace MobileTopup.Application.Topups.Commands.AddTopupBeneficiary
{
    public class AddTopUpBeneficiaryCommandValidator : AbstractValidator<AddTopUpBeneficiaryCommand>
    {
        public AddTopUpBeneficiaryCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Nickname).NotEmpty().MaximumLength(20);

        }
    }
}
