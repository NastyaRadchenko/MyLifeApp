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
    public class StateRepository : RepositoryBase<State>, IStateRepository
    {
        public StateRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<State>> GetStatesByUserIdAsync(Guid userId, bool trackChanges) =>
            await FindByCondition(e => e.UserId.Equals(userId), trackChanges)
            .ToListAsync();

        public async Task<State> GetStateByIdAsync(Guid stateId, bool trackChanges) =>
            await FindByCondition(e => e.Id.Equals(stateId), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateState(State state) => Create(state);
        public void DeleteState(State state) => Delete(state);
    }
}
