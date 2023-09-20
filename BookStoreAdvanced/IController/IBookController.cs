using Microsoft.AspNetCore.Mvc;
using Repository.ModelView;

namespace BookStoreAdvanced.IController
{
    public interface IBookController
    {
        public Task<IActionResult> GetAllBook();
        public Task<IActionResult> GetBookById(Guid id);
        public Task<IActionResult> GetBookByName(string name);
        public Task<IActionResult> AddOneBook(BookView bookView);
        public Task<IActionResult> AddManyBook(List<BookView> bookViews);
        public Task<IActionResult> UpdateBookById(Guid ID, BookView bookView);
        public Task<IActionResult> DeleteBookById(Guid id);
    }
}
