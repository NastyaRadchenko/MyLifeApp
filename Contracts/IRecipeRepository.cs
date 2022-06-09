using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<Recipe>> GetRecpesByUserIdAsync(Guid userId, bool trackChanges);
        Task<Recipe> GetRecipeByIdAsync(Guid recipeId, bool trackChanges);
        void CreateRecipe(Recipe recipe);
        void DeleteRecipe(Recipe recipe);
    }
}
