using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IStageRepository
    {
        Task<IEnumerable<Stage>> GetStagesByRecipeIdAsync(Guid recipeId, bool trackChanges);
        Task<Stage> GetStageByIdAsync(Guid stageId, bool trackChanges);
        void CreateStage(Stage stage);
        void DeleteStage(Stage stage);
    }
}
