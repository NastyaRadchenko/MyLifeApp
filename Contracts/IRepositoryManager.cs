using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryManager
    {
        IUserRepository User { get; }
        IRoleRepository Role { get; }
        IDiaryRepository Diary { get; }
        IStateRepository State { get; }
        Task SaveAsync();
    }
}
