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
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _repository.BookCategory.GetAllCategoriesAsync(trackChanges: false);
            return Ok(categories);
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
    }
}
