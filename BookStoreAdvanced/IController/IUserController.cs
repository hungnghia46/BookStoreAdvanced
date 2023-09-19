using Microsoft.AspNetCore.Mvc;
using Repository.ModelView;

namespace BookStoreAdvanced.IController
{
    public interface IUserController
    {
        Task<IActionResult> GetAllUser();
        //public Task<IActionResult> GetUserById(Guid id);
        //public Task<IActionResult> GetUserByName(string name);
        public Task<IActionResult> AddOneUser(UserView userView);
        Task<IActionResult> AddManyUser(List<UserView> userViews);
        //public Task<IActionResult> UpdateUserById(Guid ID, BookView bookView);
        //public Task<IActionResult> DeleteUserById(Guid id);
    }
}
