using System.Collections.Generic;
using Bank.API.Models;
using Bank.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IBankRepository _repository;

        public UsersController(IBankRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = _repository.GetUsers();
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public ActionResult<User> GetUser(int userId)
        {
            var user = _repository.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] User user)
        {
            _repository.CreateUser(user);
            _repository.SaveChanges();

            return CreatedAtAction(nameof(GetUser), new { userId = user.Id }, user);
        }

        [HttpDelete("{userId}")]
        public ActionResult DeleteUser(int userId)
        {
            var user = _repository.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }

            _repository.DeleteUser(userId);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}