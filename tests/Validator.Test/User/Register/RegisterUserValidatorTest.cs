using CommomTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;

namespace Validator.Test.User.Register
{
    public class RegisterUserValidatorTest
    {
        [Fact]
        public void Success() 
        {
            RegisterUserValidator validator = new();

            var request = RequestRegisterUserJsonBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Name_Empty()
        {
            RegisterUserValidator validator = new();

            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage == ResourceMessagesException.Name_Empty);
        }

        [Fact]
        public void Error_Email_Empty()
        {
            RegisterUserValidator validator = new();

            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = string.Empty;
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage == ResourceMessagesException.Email_Empty);
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            RegisterUserValidator validator = new();

            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = "email.com";
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage == ResourceMessagesException.Email_Invalid);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Error_ShortPassword(int passwordLength)
        {
            RegisterUserValidator validator = new();

            var request = RequestRegisterUserJsonBuilder.Build(passwordLength);

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage == ResourceMessagesException.ShortPassword);
        }
    }
}
