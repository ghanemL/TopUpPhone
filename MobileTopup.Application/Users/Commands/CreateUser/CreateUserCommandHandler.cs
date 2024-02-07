using MediatR;
using MobileTopup.Application.Common.Interfaces.Persistance;
using MobileTopup.Domain.UserAggregate;
using MobileTopup.Domain.Common.Errors;
using ErrorOr;

namespace MobileTopup.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<User>>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetByName(request.Name);

            if (user != null)
            {
                return Errors.User.UserAlreadyExist;
            }

            user = User.Create(request.Name);

            await _userRepository.SaveAsync(user);

            return user;
        }
    }
}
