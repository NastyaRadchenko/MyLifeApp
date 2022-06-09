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
    public class StageRepository : RepositoryBase<Stage>, IStageRepository
    {
        public StageRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Stage>> GetStagesByRecipeIdAsync(Guid recipeId, bool trackChanges) =>
           await FindByCondition(e => e.RecipeId.Equals(recipeId), trackChanges)
           .OrderBy(e => e.StageNumber)
           .ToListAsync();

        public async Task<Stage> GetStageByIdAsync(Guid stageId, bool trackChanges) =>
            await FindByCondition(e => e.Id.Equals(stageId), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateStage(Stage stage) => Create(stage);
        public void DeleteStage(Stage stage) => Delete(stage);
    }
}
