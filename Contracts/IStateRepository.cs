using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IStateRepository
    {
        Task<IEnumerable<State>> GetStatesByUserIdAsync(Guid userId, bool trackChanges);
        Task<State> GetStateByIdAsync(Guid stateId, bool trackChanges);
        void CreateState(State state);
        void DeleteState(State state);
    }
}
