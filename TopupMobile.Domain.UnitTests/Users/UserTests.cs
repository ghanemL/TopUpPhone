using FluentAssertions;
using MobileTopup.Domain.UserAggregate;
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
    }
}
