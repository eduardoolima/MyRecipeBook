using Moq;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Repositories
{
    public class UnitOfWorkBuilder
    {
        public static IUnitOfWork Build()
        {
            Mock<IUnitOfWork> mock = new();
            return mock.Object;
        }
    }
}
