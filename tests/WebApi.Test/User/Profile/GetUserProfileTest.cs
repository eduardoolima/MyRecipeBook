using CommomTestUtilities.Tokens;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.User.Profile
{
    public class GetUserProfileTest : MyRecipeBookClassFixture
    {
        readonly string METHOD = "user";

        readonly string _name;
        readonly string _email;
        readonly Guid _userId;

        public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _name = factory.GetUserName();
            _email = factory.GetUserEmail();
            _userId = factory.GetUserId();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
            var response = await DoGet(METHOD, token: token);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);            

            responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_name);
            responseData.RootElement.GetProperty("email").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_email);
        }
    }
}
