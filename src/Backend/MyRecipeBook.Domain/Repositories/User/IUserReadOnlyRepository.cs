namespace MyRecipeBook.Domain.Repositories.User
{
    public interface IUserReadOnlyRepository
    {
        public Task<bool> ExistActiveUserEmail(string email);
        public Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier);
        public Task<Entities.User?> GetByEmainAndPassword(string email, string password);
    }
}
