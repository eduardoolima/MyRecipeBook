using Moq;
using MyRecipeBook.Domain.Repositories.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Repositories
{
    public class UserUpdateOnlyRepositoryBuilder
    {
        readonly Mock<IUserUpdateOnlyRepository> _repository;

        public UserUpdateOnlyRepositoryBuilder() => _repository = new Mock<IUserUpdateOnlyRepository>();

        public UserUpdateOnlyRepositoryBuilder GetById(MyRecipeBook.Domain.Entities.User user)
        {
            _repository.Setup(x => x.GetById(user.Id)).ReturnsAsync(user);
            return this;
        }

        public IUserUpdateOnlyRepository Build() => _repository.Object;
    }
}
