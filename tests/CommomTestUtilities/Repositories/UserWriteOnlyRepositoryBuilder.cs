using Moq;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Domain.Repositories
{
    public class UserWriteOnlyRepositoryBuilder
    {
        public static IUserWriteOnlyRepository Build()
        {
            Mock<IUserWriteOnlyRepository> mock = new();
            return mock.Object;
        }
    }
}
