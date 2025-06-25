using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Register
{
    public class RegisterUserUseCaseTest
    {
        static RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
            if (!string.IsNullOrEmpty(email))
                userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(email, true);
            return new RegisterUserUseCase(
                UserWriteOnlyRepositoryBuilder.Build(), userReadOnlyRepositoryBuilder.Build(),
                UnitOfWorkBuilder.Build(), MapperBuilder.Build(), PasswordEncripterBuilder.Build()
            );
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();            

            RegisterUserUseCase useCase = CreateUseCase();

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);

        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var userCase = CreateUseCase(request.Email);

            Func<Task> act = async () => await userCase.Execute(request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(e => e.ErrorMessages.Count == 1 && e.ErrorMessages.Contains(ResourceMessagesException.EmailAlreadyRegistered));
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;
            var userCase = CreateUseCase();

            Func<Task> act = async () => await userCase.Execute(request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(e => e.ErrorMessages.Count == 1 && e.ErrorMessages.Contains(ResourceMessagesException.Name_Empty));
        }
    }
}
