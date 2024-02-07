﻿using MobileTopup.Application.Users.Commands.CreateUser;
using MobileTopup.Domain.UserAggregate;
using MobileTopup.IntegrationTests;
using Xunit;
using Shouldly;

namespace MobileTopup.Tests.CreateUser
{
    [Collection(nameof(SliceFixture))]
    public class CreateTests
    {
        private readonly SliceFixture _fixture;

        public CreateTests(SliceFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_create_user()
        {
            var cmd = new CreateUserCommand("Golden");

            var user = await _fixture.SendAsync(cmd);

            var result = await _fixture.FindAsync<User>(user.Value.Id);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(cmd.Name);
        }
    }
}
