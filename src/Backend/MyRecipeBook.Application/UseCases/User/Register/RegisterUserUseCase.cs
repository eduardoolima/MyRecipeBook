using AutoMapper;
using FluentValidation.Results;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
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

        public RegisterUserUseCase
        (
            IUserWriteOnlyRepository writeOnlyRepository,
            IUserReadOnlyRepository readOnlyRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            PasswordEncripter passwordEncripter
        )
        {
            _writeOnlyRepository = writeOnlyRepository;
            _readOnlyRepository = readOnlyRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordEncripter = passwordEncripter;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            await Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request);            
            user.Password = _passwordEncripter.Encrypt(request.Password);

            await _writeOnlyRepository.Add(user);
            await _unitOfWork.Commit();

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
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
