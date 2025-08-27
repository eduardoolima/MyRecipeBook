using Bogus;
using MyRecipeBook.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Requests
{
    public class RequestChangePasswordJsonBuilder
    {
        public static RequestChangePasswordJson Build(int passwordLength = 10)
        {
            return new Faker<RequestChangePasswordJson>()
                .RuleFor(u => u.Password, (f) => f.Internet.Password())
                .RuleFor(u => u.NewPassword, (f) => f.Internet.Password(passwordLength));
        }
    }
}
