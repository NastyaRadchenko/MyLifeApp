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
    public class RecipeRepository : RepositoryBase<Recipe>, IRecipeRepository
    {
        public RecipeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Recipe>> GetRecpesByUserIdAsync(Guid userId, bool trackChanges) =>
            await FindByCondition(e => e.UserId.Equals(userId), trackChanges)
            .ToListAsync();

        public async Task<Recipe> GetRecipeByIdAsync(Guid recipeId, bool trackChanges) =>
            await FindByCondition(e => e.Id.Equals(recipeId), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateRecipe(Recipe recipe) => Create(recipe);
        public void DeleteRecipe(Recipe recipe) => Delete(recipe);
    }
}
