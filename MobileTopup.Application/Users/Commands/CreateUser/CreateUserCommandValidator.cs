using FluentValidation;

namespace MobileTopup.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(20);
            //RuleFor(x => x.Benficiaries).NotEmpty().Must(requests => requests.Count <= 5);

        }
    }
}
