using Bogus;
using MyRecipeBook.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Requests
{
    public class RequestRegisterUserJsonBuilder
    {
        public static RequestRegisterUserJson Build(int passwordLength = 10)
        {
            return new Faker<RequestRegisterUserJson>()
                .RuleFor(user => user.Name, f => f.Person.FullName)
                .RuleFor(user => user.Email, (f, u) => f.Internet.Email(u.Name))
                .RuleFor(user => user.Password, f => f.Internet.Password(passwordLength));
        }
    }
}
