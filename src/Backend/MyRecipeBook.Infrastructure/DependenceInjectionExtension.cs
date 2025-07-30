using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Validator;
using MyRecipeBook.Infrastructure.Services.LoggedUser;
using System.Reflection;

namespace MyRecipeBook.Infrastructure
{
    public static class DependenceInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services);
            AddLoggedUser(services);
            AddTokens(services, configuration);

            if (configuration.IsUnitTestEnvironment())
                return;
            string connectionString = configuration.ConnectionString();
            AddDbContext(services, connectionString);
            AddFluentMigrator(services, connectionString);                  
        }

        static void AddDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MyRecipeBookDbContext>(dbContextOptions =>
            {
                dbContextOptions.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 4, 0)));
            });
        }

        static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnityOfWork>();
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        }

        static void AddFluentMigrator(IServiceCollection services, string connectionString)
        {
            services.AddFluentMigratorCore()
                .ConfigureRunner(options => options
                    .AddMySql5()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure")).For.All());
        }

        static void AddTokens(IServiceCollection services, IConfiguration configuration)
        {
            var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
            var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

            services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
            services.AddScoped<IAcessTokenValidator>(option => new JwtTokenValidator(signingKey!));
        }

        static void AddLoggedUser(IServiceCollection services)
        {
            services.AddScoped<ILoggedUser, LoggedUser>();
        }
    }
}
