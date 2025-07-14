using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions.ExceptionsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        readonly IUserReadOnlyRepository _repository;
        readonly PasswordEncripter _passwordEncripter;

        public DoLoginUseCase(IUserReadOnlyRepository repository, PasswordEncripter passwordEncripter)
        {
            _repository = repository;
            _passwordEncripter = passwordEncripter;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
        {
            var user = await _repository.GetByEmainAndPassword(request.Email, _passwordEncripter.Encrypt(request.Password)) ?? throw new InvalidLoginException();

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
            };
        }
    }
}
