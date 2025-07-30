using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceMessagesException.Name_Empty);
        RuleFor(request => request.Email).NotEmpty().WithMessage(ResourceMessagesException.Email_Empty);

        When(request => request.Email.NotEmpty(), () =>
        {
            RuleFor(request => request.Email).EmailAddress().WithMessage(ResourceMessagesException.Email_Invalid);
        });
    }
}