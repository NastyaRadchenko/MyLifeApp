using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPurchaseCategoryRepository
    {
        Task<IEnumerable<PurchaseCategory>> GetPurchaseCategoriesByUserIdAsync(Guid userId, bool trackChanges);
        Task<PurchaseCategory> GetPurchaseCategoryByIdAsync(Guid categoryId, bool trackChanges);
        void CreatePurchaseCategory(PurchaseCategory category);
        void DeletePurchaseCategory(PurchaseCategory category);
    }
}
