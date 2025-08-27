using FluentValidation;
using FluentValidation.Validators;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Application.SharedValidators
{
    public class PasswordValidator<T> : PropertyValidator<T, string>
    {
        const string messageFormatterNameParamter = "ErrorMessage";
        public override string Name => "PasswordValidator";

        public override bool IsValid(ValidationContext<T> context, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                context.MessageFormatter.AppendArgument(messageFormatterNameParamter, ResourceMessagesException.EmptyPassword);
                return false;
            }

            if (password.Length < 6)
            {
                context.MessageFormatter.AppendArgument(messageFormatterNameParamter, ResourceMessagesException.ShortPassword);
                return false;
            }

            return true;
        }

        protected override string GetDefaultMessageTemplate(string errorCode) => "{" + messageFormatterNameParamter + "}";
    }
}
