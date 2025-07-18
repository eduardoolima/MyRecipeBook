﻿using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommomTestUtilities.Requests
{
    public class RequestLoginJsonBuilder
    {
        public static RequestLoginJson Build()
        {
            return new Faker<RequestLoginJson>()
                .RuleFor(user => user.Email, f => f.Internet.Email())
                .RuleFor(user => user.Password, f => f.Internet.Password());
        }
    }
}
