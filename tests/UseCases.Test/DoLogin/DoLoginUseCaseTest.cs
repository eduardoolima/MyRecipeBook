using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Entities;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using CommomTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Fact]
        public async Task Success() 
        {
            (var user, var password) = UserBuilder.Build();
            var useCase = CreateUseCase(user);

            var result = await useCase.Execute(new RequestLoginJson
            {
                Email = user.Email,
                Password = password
            });

            result.Should().NotBeNull();
            result.Tokens.Should().NotBeNull();
            result.Name.Should().NotBeNullOrEmpty().And.NotBeNullOrWhiteSpace().And.Be(user.Name);
            result.Tokens.AccessToken.Should().NotBeNullOrEmpty().And.NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Error_Invalid_User()
        {
            var request = RequestLoginJsonBuilder.Build();
            
            var useCase = CreateUseCase();

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<InvalidLoginException>()
                .Where(e => e.Message.Equals(ResourceMessagesException.Email_or_Password_Invalid));
        }


        public static DoLoginUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
        {
            var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();

            if(user is not null)
                userReadOnlyRepositoryBuilder.GetByEmainAndPassword(user);

            return new DoLoginUseCase(userReadOnlyRepositoryBuilder.Build(), PasswordEncripterBuilder.Build(), JwtTokenGeneratorBuilder.Build());
        }

    }
}
