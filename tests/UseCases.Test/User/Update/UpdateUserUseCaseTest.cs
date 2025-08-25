using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUserBuilder;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Update
{
    public class UpdateUserUseCaseTest
    {
        [Fact]
        public async Task Success() 
        {
            (var user, _) = UserBuilder.Build();
            var request = RequestUpdateUserJsonBuilder.Build();
            var useCase = CreateUseCase(user);
            Func<Task> action = async () => await useCase.Execute(request);
            await action.Should().NotThrowAsync();
            user.Name.Should().Be(request.Name);
            user.Email.Should().Be(request.Email);
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            (var user, _) = UserBuilder.Build();
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;
            var useCase = CreateUseCase(user);
            Func<Task> action = async () => await useCase.Execute(request);
            (await action.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(e => e.ErrorMessages.Count == 1 && e.ErrorMessages.Contains(ResourceMessagesException.Name_Empty));
            user.Name.Should().NotBe(request.Name);
            user.Email.Should().NotBe(request.Email);
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            (var user, _) = UserBuilder.Build();
            var request = RequestUpdateUserJsonBuilder.Build();
            var useCase = CreateUseCase(user, request.Email);
            Func<Task> action = async () => { await useCase.Execute(request); };
            await action.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(e => e.ErrorMessages.Count == 1 && e.ErrorMessages.Contains(ResourceMessagesException.EmailAlreadyRegistered));
            user.Name.Should().NotBe(request.Name);
            user.Email.Should().NotBe(request.Email);
        }

        static UpdateUserUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, string? email = null)
        {
           var unitOfWork = UnitOfWorkBuilder.Build();
            var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
            if(!string.IsNullOrEmpty(email))
                userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(email!, true);
            return new UpdateUserUseCase(loggedUser, userUpdateRepository, userReadOnlyRepositoryBuilder.Build(), unitOfWork);
        }
    }
}
