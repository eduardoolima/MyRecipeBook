using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserUseCase
    {
        readonly IUserWriteOnlyRepository _writeOnlyRepository;
        readonly IUserReadOnlyRepository _readOnlyRepository;
        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            Validate(request);

            var autoMapper = new AutoMapper.MapperConfiguration(options => options.AddProfile(new AutoMapping())).CreateMapper();
            PasswordEncripter passwordEncripter = new();

            var user = autoMapper.Map<Domain.Entities.User>(request);            
            user.Password = passwordEncripter.Encrypt(request.Password);

            await _writeOnlyRepository.Add(user);

            return new ResponseRegisteredUserJson
            {
                Name = request.Name,
            };
        }

        public void Validate(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
