using FluentValidation;

namespace MobileTopup.Application.Topups.Queries.GetTopUpBeneficiaries
{
    public class GetTopUpBeneficiariesQueryValidator : AbstractValidator<GetTopUpBeneficiariesQuery>
    {
        public GetTopUpBeneficiariesQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
