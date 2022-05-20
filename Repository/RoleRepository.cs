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
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<Role> GetRoleByIdAsync(Guid roleId, bool trackChanges) =>
            await FindByCondition(e => e.Id.Equals(roleId), trackChanges)
            .SingleOrDefaultAsync();
    }
}
