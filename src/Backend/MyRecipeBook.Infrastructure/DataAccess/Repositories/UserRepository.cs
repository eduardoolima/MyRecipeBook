using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories
{
    public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
    {
        private readonly MyRecipeBookDbContext _dbContext;

        public UserRepository(MyRecipeBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public async Task<bool> ExistActiveUserEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email.Equals(email) && u.Active);
        }


        public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier)
        {
            return await _dbContext.Users.AnyAsync(u => u.Id.Equals(userIdentifier) && u.Active);
        }

        public async Task<User?> GetByUserIdentifier(Guid userId)
        {
            return await _dbContext.Users.AsNoTracking()
                .FirstAsync(u => u.Id.Equals(userId) && u.Active);
        }

        public async Task<User?> GetByEmainAndPassword(string email, string password)
        {
            return await _dbContext.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Password.Equals(password) && u.Active);
        }
    }
}
