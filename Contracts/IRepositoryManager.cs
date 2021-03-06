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
        IBookCategoryRepository BookCategory { get; }
        IBookRepository Book { get; }
        IPurchaseCategoryRepository PurchaseCategory { get; }
        IPurchaseRepository Purchase { get; }
        IRecipeRepository Recipe { get; }
        IStageRepository Stage { get; }
        Task SaveAsync();
    }
}
