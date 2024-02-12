using FluentAssertions;
using MobileTopup.Api.IntegrationTests.Common;
using MobileTopup.Api.IntegrationTests.Common.WebApplicationFactory;
using MobileTopup.Application.Topups.Queries.GetTopUpBeneficiaries;
using MobileTopup.Contracts.Requests;
using MobileTopup.Contracts.Responses;
using MobileTopup.Domain.TopupOptions.Enums;
using MobileTopUp.Web.ExternalHttpClient;
using Moq;
using System.Net;
using Xunit;

namespace MobileTopup.Api.IntegrationTests.Controller
{
    [Collection(WebAppFactoryCollection.CollectionName)]
    public class TopUpControllerTests
    {
        private readonly AppHttpClient _client;
        

        public TopUpControllerTests(WebAppFactory webAppFactory)
        {
            _client = webAppFactory.CreateAppHttpClient();
            webAppFactory.ResetDatabase();
        }

        [Fact]
        public async Task CreateUser_ShouldSuccess()
        {
            var expectedName = "UserTest1";

            var createUserRequest = new CreateUserRequest() { Name = expectedName };

            var response = await _client.CreateUserAndExpectSuccessAsync(createUserRequest: createUserRequest);

            response.Name.Should().Be(expectedName);
        }

        [Fact]
        public async Task GetAvailableTopUpOptions_ReturnsOk()
        {
            var result = await _client.GetAvailableTopUpOptionsAndExpectSuccessAsync();

            result.Should().BeOfType<TopUpOptionResponse>();
            result.Options.Should().HaveCountGreaterThan(1);
        }

        [Fact]
        public async Task AddBeneficiary_ValidRequest_ReturnsOk()
        {
            var expectedNickName = "Beneficiary1";
            var user = await _client.CreateUserAndExpectSuccessAsync(new CreateUserRequest { Name = "User2" });

            var addBeneficiaryRequest = new AddTopUpBeneficiaryRequest { UserId = user.Id, Nickname = expectedNickName };
            
            var result = await _client.AddBeneficiaryExpectSuccessAsync(addBeneficiaryRequest);

            result.Beneficiaries.Should().HaveCount(1);
            result.Beneficiaries[0].Nickname.Should().Be(expectedNickName);
        }

        [Fact]
        public async Task AddBeneficiary_InValidRequest_ReturnsKo()
        {
            var user = await _client.CreateUserAndExpectSuccessAsync(new CreateUserRequest { Name = "User3" });

            var addBeneficiaryRequest = new AddTopUpBeneficiaryRequest { UserId = user.Id, Nickname = "Beneficiary1Beneficiary2" };

            var response = await _client.AddBeneficiaryExpectErrorAsync(addBeneficiaryRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetBeneficiaries_ValidUserId_ReturnsOk()
        {
            var user = await _client.CreateUserAndExpectSuccessAsync(new CreateUserRequest { Name = "User4" });

            var addBeneficiaryRequest = new AddTopUpBeneficiaryRequest { UserId = user.Id, Nickname = "Beneficiary4" };
            await _client.AddBeneficiaryExpectSuccessAsync(addBeneficiaryRequest);

            addBeneficiaryRequest.Nickname = "Beneficiary5";
            await _client.AddBeneficiaryExpectSuccessAsync(addBeneficiaryRequest);

            var getBeneficiariesRequest = new GetTopUpBeneficiariesQuery { UserId = user.Id };
            var result = await _client.GetBeneficiariesAndExpectSuccessAsync(getBeneficiariesRequest);

            result.Should().BeOfType<List<TopUpBeneficiaryResponse>>();
            result.Count.Should().Be(2);

        }

        [Fact]
        public async Task GetBeneficiaries_InValidUserId_ReturnsKO()
        {
            var user = await _client.CreateUserAndExpectSuccessAsync(new CreateUserRequest { Name = "User5" });

            var addBeneficiaryRequest = new AddTopUpBeneficiaryRequest { UserId = user.Id, Nickname = "Beneficiary6" };
            await _client.AddBeneficiaryExpectSuccessAsync(addBeneficiaryRequest);

            var getBeneficiariesRequest = new GetTopUpBeneficiariesQuery { UserId = Guid.NewGuid() };
            var response = await _client.GetBeneficiariesAndExpectErrorAsync(getBeneficiariesRequest);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ExecuteTopUp_InValidRequest_ReturnsKO()
        {
            var user = await _client.CreateUserAndExpectSuccessAsync(new CreateUserRequest { Name = "User6" });

            var addBeneficiaryRequest = new AddTopUpBeneficiaryRequest { UserId = user.Id, Nickname = "Beneficiary7" };
            user = await _client.AddBeneficiaryExpectSuccessAsync(addBeneficiaryRequest);

            addBeneficiaryRequest.Nickname = "Beneficiary8";
            user = await _client.AddBeneficiaryExpectSuccessAsync(addBeneficiaryRequest);

            var topUpRequests = new List<TopUpRequest> { new TopUpRequest { BeneficiaryId = user.Beneficiaries[0].Id, TopUpOption = TopUpOption.AED5},
            new TopUpRequest { BeneficiaryId = user.Beneficiaries[1].Id, TopUpOption = TopUpOption.AED10}};

            var executeTopUpRequest = new ExecuteTopUpRequest() { UserId = user.Id, TopUpRequests = topUpRequests };

            var response = await _client.ExecuteTopUpAsync(executeTopUpRequest);

            // balance APi unavailable 
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task ExecuteTopUp_ValidRequest_ReturnsOk()
        {
            var user = await _client.CreateUserAndExpectSuccessAsync(new CreateUserRequest { Name = "User6" });

            var addBeneficiaryRequest = new AddTopUpBeneficiaryRequest { UserId = user.Id, Nickname = "Beneficiary7" };
            user = await _client.AddBeneficiaryExpectSuccessAsync(addBeneficiaryRequest);

            addBeneficiaryRequest.Nickname = "Beneficiary8";
            user = await _client.AddBeneficiaryExpectSuccessAsync(addBeneficiaryRequest);

            var topUpRequests = new List<TopUpRequest> { new TopUpRequest { BeneficiaryId = user.Beneficiaries[0].Id, TopUpOption = TopUpOption.AED5},
            new TopUpRequest { BeneficiaryId = user.Beneficiaries[1].Id, TopUpOption = TopUpOption.AED10}};

            var executeTopUpRequest = new ExecuteTopUpRequest() { UserId = user.Id, TopUpRequests = topUpRequests };

            var response = await _client.ExecuteTopUpAsync(executeTopUpRequest);

            //response.IsSuccessStatusCode.Should().BeFalse();
            //response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

    }
}
