using CommomTestUtilities.Entities;
using CommomTestUtilities.Tokens;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Infrastructure.DataAccess;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        MyRecipeBook.Domain.Entities.User _user = default!;
        string _password = string.Empty;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test").ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MyRecipeBookDbContext>));
                if(descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
                services.AddDbContext<MyRecipeBookDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMomoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();
                StartDatabase(dbContext);
            });
        }

        public string GetUserEmail()
        {
            return _user.Email;
        }
        public string GetUserPassword()
        {
            return _password;
        }
        public string GetUserName()
        {
            return _user.Name;
        }
        public Guid GetUserId()
        {
            return _user.UserId;
        }
        void StartDatabase(MyRecipeBookDbContext dbContext)
        {
            (_user, _password) = UserBuilder.Build();
            dbContext.Database.EnsureDeleted();
            dbContext.Users.Add(_user);
            dbContext.SaveChanges();
        }
    }
}
