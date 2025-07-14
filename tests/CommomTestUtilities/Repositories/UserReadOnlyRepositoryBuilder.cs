using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

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

        public void GetByEmainAndPassword(User user)
        {
            _repository.Setup(repository => repository.GetByEmainAndPassword(user.Email, user.Password)).ReturnsAsync(user);
        }

        public IUserReadOnlyRepository Build()
        {
            return _repository.Object;
        }
    }
}
