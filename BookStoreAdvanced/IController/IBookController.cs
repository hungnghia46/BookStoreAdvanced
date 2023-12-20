using Microsoft.AspNetCore.Mvc;
using Repository.ModelView;

namespace BookStoreAdvanced.IController
{
    public interface IBookController
    {
        public Task<IActionResult> GetAllBook(int page,int pageSize);
        public Task<IActionResult> GetBookById(String id);
        public Task<IActionResult> GetBookByName(String name);
        public Task<IActionResult> AddOneBook(BookView bookView);
        public Task<IActionResult> AddManyBook(List<BookView> bookViews);
        public Task<IActionResult> UpdateBookById(String ID, BookView bookView);
        public Task<IActionResult> DeleteBookById(String id);
    }
}
