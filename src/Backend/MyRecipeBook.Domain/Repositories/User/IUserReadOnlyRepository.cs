namespace MyRecipeBook.Domain.Repositories.User
{
    public interface IUserReadOnlyRepository
    {
        public Task<bool> ExistActiveUserEmail(string email);
        public Task<Entities.User?> GetByEmainAndPassword(string email, string password);
    }
}
