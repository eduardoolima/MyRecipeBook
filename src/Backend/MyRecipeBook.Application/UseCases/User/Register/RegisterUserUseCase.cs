using AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        readonly IUserWriteOnlyRepository _writeOnlyRepository;
        readonly IUserReadOnlyRepository _readOnlyRepository;
        readonly IUnityOfWork _unityOfWork;
        readonly IMapper _mapper;
        readonly PasswordEncripter _passwordEncripter;

        public RegisterUserUseCase
        (
            IUserWriteOnlyRepository writeOnlyRepository,
            IUserReadOnlyRepository readOnlyRepository,
            IUnityOfWork unityOfWork,
            IMapper mapper,
            PasswordEncripter passwordEncripter
        )
        {
            _writeOnlyRepository = writeOnlyRepository;
            _readOnlyRepository = readOnlyRepository;
            _unityOfWork = unityOfWork;
            _mapper = mapper;
            _passwordEncripter = passwordEncripter;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            Validate(request);

            //var autoMapper = new AutoMapper.MapperConfiguration(options => options.AddProfile(new AutoMapping())).CreateMapper();


            var user = _mapper.Map<Domain.Entities.User>(request);            
            user.Password = _passwordEncripter.Encrypt(request.Password);

            await _writeOnlyRepository.Add(user);
            await _unityOfWork.Commit();

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
