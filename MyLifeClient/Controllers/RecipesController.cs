using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyLifeClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLifeClient.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ILogger<RecipesController> _logger;

        public RecipesController(ILogger<RecipesController> logger)
        {
            _logger = logger;
        }
        public IActionResult CreateRecipe()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRecipe(IFormFile imgFile, Recipe inputRecipe)
        {
            if (imgFile != null)
                inputRecipe.Picture = ByteConvert.GetBytesFromFile(imgFile);
            var responce = await RequestsToServer<Recipe>.SendPost(inputRecipe, $"https://localhost:5001/api/users/{MainUser.GetId()}/recipes");
            if (responce.IsSuccessStatusCode)
            {

                return RedirectToAction("CreateStage", new { recipeId = inputRecipe.Id });
            }
            else return RedirectToAction("CreateRecipe", "Recipes");
        }

        public IActionResult CreateStage(Guid recipeId)
        {
            ViewBag.RecipeId = recipeId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStage(IFormFile imgFile, Stage inputStage)
        {
            if (imgFile != null)
                inputStage.Picture = ByteConvert.GetBytesFromFile(imgFile);
            var result = await RequestsToServer<IEnumerable<Stage>>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/recipes/{inputStage.RecipeId}/stages");
            inputStage.StageNumber = result.Count() + 1;
            var responce = await RequestsToServer<Stage>.SendPost(inputStage, $"https://localhost:5001/api/users/{MainUser.GetId()}/recipes/{inputStage.RecipeId}/stages");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("RecipeStages", new { recipeId = inputStage.RecipeId });
            }
            else return RedirectToAction("CreateStage", "Recipes");
        }

        public async Task<IActionResult> RecipeStages(Guid recipeId)
        {
            ViewBag.RecipeId = recipeId;
            var result = await RequestsToServer<IEnumerable<Stage>>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/recipes/{recipeId}/stages");
            return View(result);
        }
        public async Task<IActionResult> Recipes()
        {
            var result = await RequestsToServer<IEnumerable<Recipe>>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/recipes");
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRecipe(Guid inputId)
        {
            var responce = await RequestsToServer<Recipe>.SendDelete($"https://localhost:5001/api/users/{MainUser.GetId()}/recipes/{inputId}");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("Recipes", "Recipes");
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStage(Guid inputId, Guid inputRecipeId)
        {
            var responce = await RequestsToServer<Stage>.SendDelete($"https://localhost:5001/api/users/{MainUser.GetId()}/recipes/{inputRecipeId}/stages/{inputId}");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("RecipeStages", new { recipeId = inputRecipeId });
            }
            else return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Recipe(Guid inputId)
        {
            var recipe = await RequestsToServer<Recipe>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/recipes/{inputId}");
            ViewBag.Recipe = recipe;
            var stages = await RequestsToServer<IEnumerable<Stage>>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/recipes/{inputId}/stages");
            return View(stages);
        }

        public async Task<IActionResult> UpdateRecipe(Guid inputId)
        {
            var recipe = await RequestsToServer<Recipe>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/recipes/{inputId}");
            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRecipe(IFormFile imgFile, Recipe inputRecipe, bool shouldDelete)
        {
            if (shouldDelete)
                inputRecipe.Picture = null;
            if (imgFile != null)
                inputRecipe.Picture = ByteConvert.GetBytesFromFile(imgFile);
            var responce = await RequestsToServer<Recipe>.SendPut(inputRecipe, $"https://localhost:5001/api/users/{MainUser.GetId()}/recipes/{inputRecipe.Id}");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("RecipeStages", new { recipeId = inputRecipe.Id });
            }
            else return  RedirectToAction("UpdateRecipe", new { inputId = inputRecipe.Id });
        }

        public async Task<IActionResult> UpdateStage(Guid inputId)
        {
            var stage = await RequestsToServer<Stage>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/recipes//stages/{inputId}");
            return View(stage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStage(IFormFile imgFile, Stage inputStage, bool shouldDelete)
        {
            if (shouldDelete)
                inputStage.Picture = null;
            if (imgFile != null)
                inputStage.Picture = ByteConvert.GetBytesFromFile(imgFile);
            var responce = await RequestsToServer<Stage>.SendPut(inputStage, $"https://localhost:5001/api/users/{MainUser.GetId()}/recipes/{inputStage.RecipeId}/stages/{inputStage.Id}");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("RecipeStages", new { recipeId = inputStage.RecipeId });
            }
            else return RedirectToAction("UpdateStage", new { inputId = inputStage.Id });
        }
    }
}
