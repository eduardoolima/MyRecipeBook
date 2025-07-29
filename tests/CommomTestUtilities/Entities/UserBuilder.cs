using Bogus;
using CommomTestUtilities.Cryptography;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Domain.Entities;

namespace CommomTestUtilities.Entities
{
    public class UserBuilder
    {
        public static (User user,string password) Build()
        {
            var passwordEncripter = PasswordEncripterBuilder.Build();
            var password = new Faker().Internet.Password();
            var user = new Faker<User>()
                .RuleFor(user => user.Id, () => 1)
                .RuleFor(user => user.Name, f => f.Person.FullName)
                .RuleFor(user => user.Email, f => f.Internet.Email())
                .RuleFor(user => user.UserId, _ => Guid.NewGuid())
                .RuleFor(user => user.Password, f => passwordEncripter.Encrypt(password))
                .RuleFor(user => user.Active, true)
                .RuleFor(user => user.CreatedOn, f => f.Date.Past());
            return (user, password);
        }
    }
}
