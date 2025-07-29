using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToDomain();
            DomainToResponse();
        }
        void RequestToDomain()
        {
            CreateMap<RequestRegisterUserJson, Domain.Entities.User>().ForMember(dest => dest.Password, opt => opt.Ignore());
        }

        void DomainToResponse()
        {
            CreateMap<Domain.Entities.User, ResponseUserProfileJson>();
        }
    }
}
