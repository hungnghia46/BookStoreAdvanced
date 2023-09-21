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
            // You can replace this with the correct time zone ID for GMT+7
            TimeZoneInfo gmtPlus7TimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); 
            User user = new User
            {
                Id = Guid.NewGuid(),
                Address = userView.Address,
                Username = userView.Username,
                Email = userView.Email,
                DateOfBirth = userView.DateOfBirth,
                FirstName = userView.FirstName,
                LastName = userView.LastName,
                Password = userView.Password,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow

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
            TimeZoneInfo gmtPlus7TimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            List<User> users = new List<User>();
            foreach (UserView item in userViews)
            {
                users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    DateOfBirth = item.DateOfBirth,
                    Email = item.Email,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Password = item.Password,
                    Username = item.Username,
                    Address = item.Address,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            await _userRepos.addManyItem(users);
            return Ok(users);
        }

    }
}
