
using FluentValidation;

namespace MobileTopup.Application.Topups.Commands.ExecuteTopup
{
    public class ExecuteTopUpCommandValidator : AbstractValidator<ExecuteTopUpCommand>
    {
        public ExecuteTopUpCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
