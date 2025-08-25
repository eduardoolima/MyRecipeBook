using CommomTestUtilities.Requests;
using CommomTestUtilities.Tokens;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update
{
    public class UpdateUserTest : MyRecipeBookClassFixture
    {
        const string METHOD = "user";
        readonly Guid _userIdentifier;

        public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserId();
        }
        [Fact]
        public async Task Success()
        {
            var request = RequestUpdateUserJsonBuilder.Build();
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
            var response = await DoPut(METHOD, request, token);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Name_Empty(string culture)
        {
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
            var response = await DoPut(METHOD, request, token, culture);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
           
        }

    }
}
