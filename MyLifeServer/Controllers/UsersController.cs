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

        [HttpGet("{userId}/role")]
        public async Task<IActionResult> GetUserRole(Guid userId)
        {
            var user = await _repository.User.GetUserByIdAsync(userId, trackChanges: false);
            var role = await _repository.Role.GetRoleByIdAsync(user.RoleId, trackChanges: false);

            return Ok(role.Name);
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
    }
}
