using BookStoreAdvanced.IController;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Repository.Model;
using Repository.ModelView;
using Repository.Service;

namespace BookStoreAdvanced.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase, IBookController
    {
        public IRepository<Book> _bookRepos;
        public BookController(IMongoClient client)
        {
            _bookRepos = new Repository<Book>(client, "BookStoreDB", "Books");
        }
        /// <summary>
        /// Get the list all Book
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get-All-Book")]
        public async Task<IActionResult> GetAllBook()
        {
            IEnumerable<Book> books = await _bookRepos.getAllAsync();
            return Ok(books);
        }
        /// <summary>
        /// Add one new book
        /// </summary>
        /// <param name="bookView"></param>
        /// <returns></returns>
        [HttpPost("Add-One-Book")]
        public async Task<IActionResult> AddOneBook(BookView bookView)
        {
            Book book = new Book
            {
                Id = Guid.NewGuid(),
                Author = bookView.Author,
                CreatedAt = bookView.CreatedAt,
                Description = bookView.Description,
                Genre = bookView.Genre,
                ImageUrl = bookView.ImageUrl,
                InventoryQuantity = bookView.InventoryQuantity,
                Price = bookView.Price,
                ISBN = bookView.ISBN,
                PublicationYear = bookView.PublicationYear,
                Title = bookView.Title,
                UpdatedAt = bookView.UpdatedAt,
            };
            await _bookRepos.addOneItem(book);
            return Ok(book);
        }
        /// <summary>
        /// Add A array of book
        /// </summary>
        /// <param name="bookViews"></param>
        /// <returns></returns>
        [HttpPost("Add-Many-Book")]
        public async Task<IActionResult> AddManyBook(List<BookView> bookViews)
        {
            List<Book> bookList = new List<Book>();
            foreach (BookView item in bookViews)
            {
                bookList.Add(new Book
                {
                    Id = Guid.NewGuid(),
                    Author = item.Author,
                    CreatedAt = item.CreatedAt,
                    Description = item.Description,
                    Genre = item.Genre,
                    ImageUrl = item.ImageUrl,
                    InventoryQuantity = item.InventoryQuantity,
                    ISBN = item.ISBN,
                    Price = item.Price,
                    PublicationYear = item.PublicationYear,
                    Title = item.Title
                });
            }
            await _bookRepos.addManyItem(bookList);
            return Ok(bookList);
        }

    }
}
