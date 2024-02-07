using ErrorOr;
using MediatR;
using MobileTopup.Domain.UserAggregate;

namespace MobileTopup.Application.Users.Commands.CreateUser
{
    public record CreateUserCommand(
        string? Name) : IRequest<ErrorOr<User>>;
}
