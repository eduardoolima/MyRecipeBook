using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisterUserValidator() 
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.Name_Empty);
            RuleFor(user => user.Email).NotEmpty().WithMessage("Email obrigatório");
            RuleFor(user => user.Email).EmailAddress().WithMessage("Email Inválido");
            RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(6).WithMessage("A senha deve ter no minimo 6 caracteres");
        }
    }
}
