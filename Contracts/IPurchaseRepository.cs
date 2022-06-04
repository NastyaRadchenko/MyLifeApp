using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPurchaseRepository
    {
        Task<IEnumerable<Purchase>> GetPurchasesByCategoryAsync(Guid userId, Guid categoryId, bool trackChanges);
        Task<IEnumerable<Purchase>> GetPurchasesByUserIdAsync(Guid userId, bool trackChanges);
        Task<Purchase> GetPurchaseByIdAsync(Guid purchaseId, bool trackChanges);
        void CreatePurchase(Purchase purchase);
        void DeletePurchase(Purchase purchase);
    }
}
