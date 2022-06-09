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
    public class PurchaseCategoryRepository : RepositoryBase<PurchaseCategory>, IPurchaseCategoryRepository
    {
        public PurchaseCategoryRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<PurchaseCategory>> GetPurchaseCategoriesByUserIdAsync(Guid userId, bool trackChanges) =>
            await FindByCondition(e => e.UserId.Equals(userId), trackChanges)
            .ToListAsync();

        public async Task<PurchaseCategory> GetPurchaseCategoryByIdAsync(Guid categoryId, bool trackChanges) =>
            await FindByCondition(e => e.Id.Equals(categoryId), trackChanges)
            .SingleOrDefaultAsync();

        public void CreatePurchaseCategory(PurchaseCategory category) => Create(category);
        public void DeletePurchaseCategory(PurchaseCategory category) => Delete(category);
    }
}
