using MobileTopup.Application.Users.Commands.CreateUser;
using MobileTopup.Domain.UserAggregate;
using MobileTopup.IntegrationTests;
using Xunit;
using Shouldly;

namespace MobileTopup.Tests.Users
{
    [Collection(nameof(SliceFixture))]
    public class CreateUserCommandTests
    {
        private readonly SliceFixture _fixture;

        public CreateUserCommandTests(SliceFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_create_user()
        {
            var cmd = new CreateUserCommand("Golden");

            var user = await _fixture.SendAsync(cmd);

            var result = await _fixture.FindAsync<User>(user.Value.Id);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(cmd.Name);
        }

        [Fact]
        public async Task Should_not_create_duplicate_user()
        {
            var existingUser = User.Create("ExistingUser");
            await _fixture.InsertAsync(existingUser);

            var cmd = new CreateUserCommand(existingUser.Name);

            var result = await _fixture.SendAsync(cmd);

            result.Errors.First().Code.ShouldBe(Domain.Common.Errors.Errors.User.UserAlreadyExist.Code);
        }
    }
}
