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
    public class PurchaseRepository : RepositoryBase<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Purchase>> GetPurchasesByCategoryAsync(Guid userId, Guid categoryId, bool trackChanges) =>
            await FindByCondition(e => e.UserId.Equals(userId) && e.CategoryId.Equals(categoryId), trackChanges)
            .ToListAsync();

        public async Task<IEnumerable<Purchase>> GetPurchasesByUserIdAsync(Guid userId, bool trackChanges) =>
            await FindByCondition(e => e.UserId.Equals(userId), trackChanges)
            .OrderBy(e => e.Date)
            .ToListAsync();

        public async Task<Purchase> GetPurchaseByIdAsync(Guid purchaseId, bool trackChanges) =>
            await FindByCondition(e => e.Id.Equals(purchaseId), trackChanges)
            .SingleOrDefaultAsync();

        public void CreatePurchase(Purchase purchase) => Create(purchase);
        public void DeletePurchase(Purchase purchase) => Delete(purchase);
    }
}
