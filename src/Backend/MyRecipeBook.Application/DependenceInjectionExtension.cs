using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Application.UseCases.User.Profile;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Application.UseCases.User.Update;

namespace MyRecipeBook.Application
{
    public static class DependenceInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddAutoMapper(services);
            AddUseCases(services);
            AddPasswordEncrypter(services, configuration);
        }
        static void AddAutoMapper(IServiceCollection services)
        {
            services.AddScoped(option => new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapping());
            }).CreateMapper());
        }
                        
        static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
            services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        }

        static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
        {
            var addtionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");
            services.AddScoped(option => new PasswordEncripter(addtionalKey!));
        }
    }
}
