using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges) =>
        await FindAll(trackChanges)
         .OrderBy(c => c.Name)
         .ToListAsync();

        public async Task<User> GetUserAsync(Guid userId, bool trackChanges) =>
            await FindByCondition(e => e.Id.Equals(userId), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateUser(User user) => Create(user);
        public void DeleteUser(User user) => Delete(user);
    }
}
