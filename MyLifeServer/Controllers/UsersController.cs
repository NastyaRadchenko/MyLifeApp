using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLifeServer.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepositoryManager _repository;

        public UsersController(IRepositoryManager repository)
        {
            _repository = repository;
        }

        //users
        
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repository.User.GetAllUsersAsync(trackChanges: false);
            return Ok(users);
        }

        [HttpGet("emails/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _repository.User.GetUserByEmailAsync(email, trackChanges: false);
            if (user == null) return BadRequest($"User with email: {email} doesn't exist.");
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            var existedUser = await _repository.User.GetUserByEmailAsync(user.Email, trackChanges: false);
            if(existedUser != null)
            {
                return BadRequest("User with this Email already exist.");
            }
            user.RoleId = new Guid("1bc0c9fc-0cce-4256-8133-60717ff63d22");
            _repository.User.CreateUser(user);
            await _repository.SaveAsync();
            return Ok(user);
        }

        //roles

        [HttpGet("{userId}/role")]
        public async Task<IActionResult> GetUserRole(Guid userId)
        {
            var user = await _repository.User.GetUserByIdAsync(userId, trackChanges: false);
            var role = await _repository.Role.GetRoleByIdAsync(user.RoleId, trackChanges: false);

            return Ok(role.Name);
        }

        //diary

        [HttpGet("{userId}/diary")]
        public async Task<IActionResult> GetDiaryEntries(Guid userId)
        {
            var entries = await _repository.Diary.GetEntriesByUserIdAsync(userId, trackChanges: false);
            return Ok(entries);
        }


        [HttpGet("{userId}/diary/{entryId}")]
        public async Task<IActionResult> GetDiaryEntry(Guid entryId)
        {
            var entry = await _repository.Diary.GetEntryByIdAsync(entryId, trackChanges: false);
            return Ok(entry);
        }

        [HttpPost("{userId}/diary")]
        public async Task<IActionResult> CreateDiaryEntry(Guid userId, [FromBody]DiaryEntry entry)
        {
            entry.UserId = userId;
            _repository.Diary.CreateEntry(entry);
            await _repository.SaveAsync();
            return Ok(entry);
        }

        [HttpDelete("{userId}/diary/{entryId}")]
        public async Task<IActionResult> DeleteDiaryEntry(Guid entryId)
        {
            var entry = await _repository.Diary.GetEntryByIdAsync(entryId, trackChanges: false);
            _repository.Diary.DeleteEntry(entry);
            await _repository.SaveAsync();
            return Ok();
        }

        [HttpPut("{userId}/diary/{entryId}")]
        public async Task<IActionResult> UpdateEntryById(Guid entryId, [FromBody] DiaryEntry inputEntry)
        {
            var entryFromDb = await _repository.Diary.GetEntryByIdAsync(entryId, trackChanges: false);
            if(inputEntry == null)
            {
                return BadRequest("Input entry is null");
            }
            _repository.Diary.DeleteEntry(entryFromDb);
            _repository.Diary.CreateEntry(inputEntry);
            await _repository.SaveAsync();
            return Ok(inputEntry);
        }

        //states

        [HttpGet("{userId}/weigthControl")]
        public async Task<IActionResult> GetStates(Guid userId)
        {
            var states = await _repository.State.GetStatesByUserIdAsync(userId, trackChanges: false);
            return Ok(states);
        }


        [HttpGet("{userId}/weigthControl/{stateId}")]
        public async Task<IActionResult> GetState(Guid stateId)
        {
            var state = await _repository.State.GetStateByIdAsync(stateId, trackChanges: false);
            return Ok(state);
        }

        [HttpPost("{userId}/weigthControl")]
        public async Task<IActionResult> CreateState(Guid userId, [FromBody] State state)
        {
            _repository.State.CreateState(state);
            await _repository.SaveAsync();
            return Ok(state);
        }

        [HttpDelete("{userId}/weigthControl/{stateId}")]
        public async Task<IActionResult> DeleteState(Guid stateId)
        {
            var state = await _repository.State.GetStateByIdAsync(stateId, trackChanges: false);
            _repository.State.DeleteState(state);
            await _repository.SaveAsync();
            return Ok();
        }

        [HttpPut("{userId}/weigthControl/{stateId}")]
        public async Task<IActionResult> UpdateStateById(Guid stateId, [FromBody] State inputState)
        {
            var stateFromDb = await _repository.State.GetStateByIdAsync(stateId, trackChanges: false);
            if (inputState == null)
            {
                return BadRequest("Input state is null");
            }
            _repository.State.DeleteState(stateFromDb);
            _repository.State.CreateState(inputState);
            await _repository.SaveAsync();
            return Ok(inputState);
        }

        //Books

        [HttpGet("{userId}/library")]
        public async Task<IActionResult> GetBooks(Guid userId)
        {
            var books = await _repository.Book.GetBooksByUserIdAsync(userId, trackChanges: false);
            return Ok(books);
        }

        [HttpGet("{userId}/library/categories")]
        public async Task<IActionResult> GetBookCategories()
        {
            var categories = await _repository.BookCategory.GetAllCategoriesAsync(trackChanges: false);
            return Ok(categories);
        }

        [HttpGet("{userId}/library/categories/{categoryId}")]
        public async Task<IActionResult> GetBooksByCategory(Guid userId, Guid categoryId)
        {
            var books = await _repository.Book.GetBooksByCategoryAsync(userId, categoryId, trackChanges: false);
            return Ok(books);
        }


        [HttpGet("{userId}/library/{bookId}")]
        public async Task<IActionResult> GetBook(Guid bookId)
        {
            var book = await _repository.Book.GetBookByIdAsync(bookId, trackChanges: false);
            return Ok(book);
        }

        [HttpPost("{userId}/library")]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            _repository.Book.CreateBook(book);
            await _repository.SaveAsync();
            return Ok(book);
        }

        [HttpDelete("{userId}/library/{bookId}")]
        public async Task<IActionResult> DeleteBook(Guid bookId)
        {
            var book = await _repository.Book.GetBookByIdAsync(bookId, trackChanges: false);
            _repository.Book.DeleteBook(book);
            await _repository.SaveAsync();
            return Ok();
        }

        [HttpPut("{userId}/library/{bookId}")]
        public async Task<IActionResult> UpdateBookById(Guid bookId, [FromBody] Book inputBook)
        {
            var bookFromDb = await _repository.Book.GetBookByIdAsync(bookId, trackChanges: false);
            if (inputBook == null)
            {
                return BadRequest("Input book is null");
            }
            _repository.Book.DeleteBook(bookFromDb);
            _repository.Book.CreateBook(inputBook);
            await _repository.SaveAsync();
            return Ok(inputBook);
        }

        //Purchases

        [HttpGet("{userId}/expenses")]
        public async Task<IActionResult> GetPurchases(Guid userId)
        {
            var purchase = await _repository.Purchase.GetPurchasesByUserIdAsync(userId, trackChanges: false);
            return Ok(purchase);
        }

        [HttpGet("{userId}/expenses/categories")]
        public async Task<IActionResult> GetPurchaseCategories(Guid userId)
        {
            var categories = await _repository.PurchaseCategory.GetPurchaseCategoriesByUserIdAsync(userId, trackChanges: false);
            return Ok(categories);
        }

        [HttpGet("{userId}/expenses/categories/{categoryId}/purchases")]
        public async Task<IActionResult> GetPurchasesByCategory(Guid userId, Guid categoryId)
        {
            var purchases = await _repository.Purchase.GetPurchasesByCategoryAsync(userId, categoryId, trackChanges: false);
            return Ok(purchases);
        }

        [HttpGet("{userId}/expenses/categories/{categoryId}")]
        public async Task<IActionResult> GetCategory(Guid categoryId)
        {
            var category = await _repository.PurchaseCategory.GetPurchaseCategoryByIdAsync(categoryId, trackChanges: false);
            return Ok(category);
        }

        [HttpGet("{userId}/expenses/{purchesId}")]
        public async Task<IActionResult> GetPurchase(Guid purchaseId)
        {
            var purchase = await _repository.Purchase.GetPurchaseByIdAsync(purchaseId, trackChanges: false);
            return Ok(purchase);
        }

        [HttpPost("{userId}/expenses")]
        public async Task<IActionResult> CreatePurchase([FromBody] Purchase purchase)
        {
            _repository.Purchase.CreatePurchase(purchase);
            await _repository.SaveAsync();
            return Ok(purchase);
        }

        [HttpPost("{userId}/expenses/categories")]
        public async Task<IActionResult> CreateCategory([FromBody] PurchaseCategory purchaseCategory)
        {
            _repository.PurchaseCategory.CreatePurchaseCategory(purchaseCategory);
            await _repository.SaveAsync();
            return Ok(purchaseCategory);
        }

        [HttpDelete("{userId}/expenses/{purchesId}")]
        public async Task<IActionResult> DeletePurchase(Guid purchaseId)
        {
            var purchase = await _repository.Purchase.GetPurchaseByIdAsync(purchaseId, trackChanges: false);
            _repository.Purchase.DeletePurchase(purchase);
            await _repository.SaveAsync();
            return Ok();
        }

        [HttpDelete("{userId}/expenses/categories/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            var category = await _repository.PurchaseCategory.GetPurchaseCategoryByIdAsync(categoryId, trackChanges: false);
            _repository.PurchaseCategory.DeletePurchaseCategory(category);
            await _repository.SaveAsync();
            return Ok();
        }

        [HttpPut("{userId}/expenses/{purchesId}")]
        public async Task<IActionResult> UpdatePurchaseById(Guid purchaseId, [FromBody] Purchase inputPurchase)
        {
            var purchaseFromDb = await _repository.Purchase.GetPurchaseByIdAsync(purchaseId, trackChanges: false);
            if (inputPurchase == null)
            {
                return BadRequest("Input purchase is null");
            }
            _repository.Purchase.DeletePurchase(purchaseFromDb);
            _repository.Purchase.CreatePurchase(inputPurchase);
            await _repository.SaveAsync();
            return Ok(inputPurchase);
        }

        [HttpPut("{userId}/expenses/categories/{categoryId}")]
        public async Task<IActionResult> UpdateCategory(Guid categoryId, [FromBody] PurchaseCategory inputPurchaseCategory)
        {
            var purchaseCategoryFromDb = await _repository.PurchaseCategory.GetPurchaseCategoryByIdAsync(categoryId, trackChanges: false);
            if (inputPurchaseCategory == null)
            {
                return BadRequest("Input purchase category is null");
            }
            _repository.PurchaseCategory.DeletePurchaseCategory(purchaseCategoryFromDb);
            _repository.PurchaseCategory.CreatePurchaseCategory(inputPurchaseCategory);
            await _repository.SaveAsync();
            return Ok(inputPurchaseCategory);
        }

        //Recipes

        [HttpGet("{userId}/recipes")]
        public async Task<IActionResult> GetRecipes(Guid userId)
        {
            var recipes = await _repository.Recipe.GetRecpesByUserIdAsync(userId, trackChanges: false);
            return Ok(recipes);
        }

        [HttpGet("{userId}/recipes/{recipeId}")]
        public async Task<IActionResult> GetRecipe(Guid recipeId)
        {
            var recipe = await _repository.Recipe.GetRecipeByIdAsync(recipeId, trackChanges: false);
            return Ok(recipe);
        }

        [HttpGet("{userId}/recipes/{recipeId}/stages")]
        public async Task<IActionResult> GetRecipeStages(Guid recipeId)
        {
            var stages = await _repository.Stage.GetStagesByRecipeIdAsync(recipeId, trackChanges: false);
            return Ok(stages);
        }

        [HttpGet("{userId}/recipes/{recipeId}/stages/{stageId}")]
        public async Task<IActionResult> GetRecipeStage(Guid stageId)
        {
            var stage = await _repository.Stage.GetStageByIdAsync(stageId, trackChanges: false);
            return Ok(stage);
        }

        [HttpPost("{userId}/recipes")]
        public async Task<IActionResult> CreateRecipe([FromBody] Recipe recipe)
        {
            _repository.Recipe.CreateRecipe(recipe);
            await _repository.SaveAsync();
            return Ok(recipe);
        }

        [HttpPost("{userId}/recipes/{recipeId}/stages")]
        public async Task<IActionResult> CreateRecipeStage([FromBody] Stage stage)
        {
            _repository.Stage.CreateStage(stage);
            await _repository.SaveAsync();
            return Ok(stage);
        }

        [HttpDelete("{userId}/recipes/{recipeId}")]
        public async Task<IActionResult> DeleteRecipe(Guid recipeId)
        {
            var recipe = await _repository.Recipe.GetRecipeByIdAsync(recipeId, trackChanges: false);
            _repository.Recipe.DeleteRecipe(recipe);
            await _repository.SaveAsync();
            return Ok();
        }

        [HttpDelete("{userId}/recipes/{recipeId}/stages/{stageId}")]
        public async Task<IActionResult> DeleteStage(Guid stageId)
        {
            var stage = await _repository.Stage.GetStageByIdAsync(stageId, trackChanges: false);
            _repository.Stage.DeleteStage(stage);
            await _repository.SaveAsync();
            return Ok();
        }

        [HttpPut("{userId}/recipes/{recipeId}")]
        public async Task<IActionResult> UpdateRecipeById(Guid recipeId, [FromBody] Recipe inputRecipe)
        {
            var recipeFromDb = await _repository.Recipe.GetRecipeByIdAsync(recipeId, trackChanges: false);
            if (inputRecipe == null)
            {
                return BadRequest("Input recipe is null");
            }
            _repository.Recipe.DeleteRecipe(recipeFromDb);
            _repository.Recipe.CreateRecipe(inputRecipe);
            await _repository.SaveAsync();
            return Ok(inputRecipe);
        }

        [HttpPut("{userId}/recipes/{recipeId}/stages/{stageId}")]
        public async Task<IActionResult> UpdateStageById(Guid stageId, [FromBody] Stage inputStage)
        {
            var stageFromDb = await _repository.Stage.GetStageByIdAsync(stageId, trackChanges: false);
            if (inputStage == null)
            {
                return BadRequest("Input stage is null");
            }
            _repository.Stage.DeleteStage(stageFromDb);
            _repository.Stage.CreateStage(inputStage);
            await _repository.SaveAsync();
            return Ok(inputStage);
        }
    }
}
