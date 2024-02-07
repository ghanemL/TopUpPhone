
using FluentValidation;
using MobileTopup.Domain.TopupOptions.Enums;

namespace MobileTopup.Application.Topups.Commands.ExecuteTopup
{
    public class ExecuteTopUpCommandValidator : AbstractValidator<ExecuteTopUpCommand>
    {
        public ExecuteTopUpCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            //RuleFor(x => x.Beneficiaries).NotEmpty().Must(HaveValidTopUpBeneficiaryRequests);
        }

        //private bool HaveValidTopUpBeneficiaryRequests(List<TopUpBeneficiaryCommand> requests)
        //{
        //    return requests.All(request => request.BeneficiaryId != Guid.Empty && Enum.IsDefined(typeof(TopUpOption), request.TopUpOption));
        //}
    }
}
