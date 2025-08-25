using CommomTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;

namespace Validator.Test.User.Update
{
    public class UpdateUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            UpdateUserValidator validator = new();
            var request = RequestUpdateUserJsonBuilder.Build();
            var result = validator.Validate(request);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Name_Empty()
        {
            UpdateUserValidator validator = new();
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;
            var result = validator.Validate(request);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.Name_Empty));
        }

        [Fact]
        public void Error_Email_Empty()
        {
            UpdateUserValidator validator = new();
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = string.Empty;
            var result = validator.Validate(request);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.Email_Empty));
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            UpdateUserValidator validator = new();
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "email.com";
            var result = validator.Validate(request);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.Email_Invalid));
        }
    }
}
