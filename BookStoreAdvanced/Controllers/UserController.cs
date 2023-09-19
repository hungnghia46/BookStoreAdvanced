using BookStoreAdvanced.IController;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Repository.ModelView;
using Repository.Service;

namespace BookStoreAdvanced.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase, IUserController
    {
        public IRepository<User> _userRepos;
        public UserController(IMongoClient client)
        {
            _userRepos = new Repository<User>(client, "BookStoreDB", "Users");
        }
        /// <summary>
        /// Get all user
        /// </summary>
        /// <returns>Return list of user</returns>
        [HttpGet("Get-All-User")]
        public async Task<IActionResult> GetAllUser()
        {
            IEnumerable<User> users = await _userRepos.getAllAsync();
            return Ok(users);
        }
        /// <summary>
        /// Add one new User
        /// </summary>
        /// <param name="userView"></param>
        /// <returns>List of added user</returns>
        [HttpPost("Add-One-User")]
        public async Task<IActionResult> AddOneUser(UserView userView)
        {
            User user = new User
            {
                Id = Guid.NewGuid(),
                Address = userView.Address,
                CreatedAt = DateTime.UtcNow,
                Email = userView.Email,
                DateOfBirth = DateTime.UtcNow,
                FirstName = userView.FirstName,
                LastName = userView.LastName,
                Password = userView.Password,
                UpdatedAt = DateTime.UtcNow,
                Username = userView.Username,

            };
            await _userRepos.addOneItem(user);
            return Ok(user);
        }
        /// <summary>
        /// Add many user
        /// </summary>
        /// <param name="userViews"></param>
        /// <returns></returns>
        [HttpPost("Add-Many-User")]
        public async Task<IActionResult> AddManyUser(List<UserView> userViews)
        {
            List<User> users = new List<User>();
            foreach (UserView item in userViews)
            {
                users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    DateOfBirth = DateTime.UtcNow,
                    Email = item.Email,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Password = item.Password,
                    UpdatedAt = DateTime.UtcNow,
                    Username = item.Username,
                    Address = new Address
                    {
                        City = item.Address.City,
                        State = item.Address.State,
                        Street = item.Address.Street,
                        ZipCode = item.Address.ZipCode,
                    }
                });
            }
            await _userRepos.addManyItem(users);
            return Ok(users);
        }

    }
}
