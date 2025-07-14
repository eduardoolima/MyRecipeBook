using CommomTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login
{
    public class DoLoginTest : MyRecipeBookClassFixture
    {
        readonly string method = "login";

        readonly string _email;
        readonly string _password;
        readonly string _name;

        public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _email = factory.GetUserEmail();
            _password = factory.GetUserPassword();
            _name = factory.GetUserName();
        }

        [Fact]
        public async Task Success()
        {
            var request = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

            var response = await DoPost(method, request);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("name").GetString().Should().Be(_name).And.NotBeNullOrWhiteSpace();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Login_Invalid(string culture)
        {
            var request = RequestLoginJsonBuilder.Build();
            
            var response = await DoPost(method, request, culture);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("Email_or_Password_Invalid", new CultureInfo(culture));
            object value = errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}