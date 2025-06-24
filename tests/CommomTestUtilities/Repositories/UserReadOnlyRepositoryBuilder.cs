using Moq;
using MyRecipeBook.Domain.Repositories.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Repositories
{
    public class UserReadOnlyRepositoryBuilder
    {
        readonly Mock<IUserReadOnlyRepository> _repository;

        public UserReadOnlyRepositoryBuilder()
        {
            _repository = new Mock<IUserReadOnlyRepository>();
        }

        public void ExistActiveUserWithEmail(string email, bool result)
        {
            _repository.Setup(repository => repository.ExistActiveUserEmail(email)).ReturnsAsync(result);
        }

        public IUserReadOnlyRepository Build()
        {
            return _repository.Object;
        }
    }
}
