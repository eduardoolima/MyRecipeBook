using AutoMapper;
using FluentValidation.Results;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        readonly IUserWriteOnlyRepository _writeOnlyRepository;
        readonly IUserReadOnlyRepository _readOnlyRepository;
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly PasswordEncripter _passwordEncripter;
        readonly IAccessTokenGenerator _accessTokenGenerator;

        public RegisterUserUseCase
        (
            IUserWriteOnlyRepository writeOnlyRepository,
            IUserReadOnlyRepository readOnlyRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            PasswordEncripter passwordEncripter,
            IAccessTokenGenerator accessTokenGenerator
        )
        {
            _writeOnlyRepository = writeOnlyRepository;
            _readOnlyRepository = readOnlyRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordEncripter = passwordEncripter;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            await Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request);            
            user.Password = _passwordEncripter.Encrypt(request.Password);
            user.UserId = Guid.NewGuid();

            await _writeOnlyRepository.Add(user);
            await _unitOfWork.Commit();

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
                Tokens = new ResponseTokensJson
                {
                    AccessToken = _accessTokenGenerator.Generate(user.UserId),
                    //RefreshToken = _accessTokenGenerator.GenerateRefreshToken(user.UserId, user.Name, user.Email)
                }
            };
        }

        public async Task Validate(RequestRegisterUserJson request)
        {
            RegisterUserValidator validator = new();
            var result = validator.Validate(request);

            bool existActiveuserEmail = await _readOnlyRepository.ExistActiveUserEmail(request.Email);

            if (existActiveuserEmail)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EmailAlreadyRegistered));
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
