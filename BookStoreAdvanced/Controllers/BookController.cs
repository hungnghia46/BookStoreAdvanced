using BookStoreAdvanced.IController;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Repository.Model;
using Repository.ModelView;
using Repository.Service;
using System.Linq.Expressions;

namespace BookStoreAdvanced.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase, IBookController
    {
        private readonly ILogger<BookController> _logger;
        private readonly IRepository<Book> _bookRepos;
        public BookController(ILogger<BookController> logger, IMongoClient client)
        {
            _logger = logger;
            _bookRepos = new Repository<Book>(client, "BookStoreDB", "Books");
        }
        /// <summary>
        /// Get the list all Book
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("Get-All-Book")]
        public async Task<IActionResult> GetAllBook()
        {
            try
            {
                IEnumerable<Book> books = await _bookRepos.getAllAsync();
                _logger.LogInformation("Retrieved {Count} books.", books.Count());
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving books.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
        /// <summary>
        /// Get a book by it Id
        /// </summary>
        /// <param name="id">The Id of book to search</param>
        /// <returns>Book</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("Get-By-Id")]
        public async Task<IActionResult> GetBookById([FromQuery] Guid id)
        {
            Expression<Func<Book, bool>> filterExpresstion = x => x.Id == id;
            IEnumerable<Book> filtedBookList = await _bookRepos.GetByFilterAsync(filterExpresstion);
            return Ok(filtedBookList);
        }
        /// <summary>
        /// Get a book by it title
        /// </summary>
        /// <param name="title">The title of book to search</param>
        /// <returns>Book</returns>
        [HttpGet("Get-Book-By-Title")]
        public async Task<IActionResult> GetBookByName([FromQuery] string title)
        {
            Expression<Func<Book, bool>> filterExpresstion = x => x.Title == title;
            IEnumerable<Book> filtedBookList = await _bookRepos.GetByFilterAsync(filterExpresstion);
            return Ok(filtedBookList);
        }
        /// <summary>
        /// Add one new book
        /// </summary>
        /// <param name="bookView"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
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
        /// <summary>
        /// update A book base on it Id
        /// </summary>
        /// <param name="ID">Id of book update</param>
        /// <param name="bookView"></param>
        /// <returns>Updated book</returns>

        [HttpPut("Update-Book-By-Id")]
        public async Task<IActionResult> UpdateBookById([FromQuery] Guid ID, BookView bookView)
        {
            var update = Builders<Book>.Update
                .Set("title", bookView.Title)
                .Set("author", bookView.Author)
                .Set("description", bookView.Description)
                .Set("genre", bookView.Genre)
                .Set("imageUrl", bookView.ImageUrl)
                .Set("publicationYear", bookView.PublicationYear)
                .Set("isbn", bookView.ISBN)
                .Set("inventoryQuantity", bookView.InventoryQuantity)
                .Set("updatedAt", DateTime.UtcNow)
                .Set("price", bookView.Price);

            Book updatedBook = await _bookRepos.updateItemByValue(ID, update);
            if (updatedBook != null)
            {
                return Ok(updatedBook);
            }
            return BadRequest();
        }
        /// <summary>
        /// Delete a book base on it Id
        /// </summary>
        /// <param name="id">Id of book</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("Delete-Book")]
        public async Task<IActionResult> DeleteBookById([FromQuery]Guid id)
        {
            bool isDelete =await _bookRepos.removeItemByValue(id);
            if (isDelete)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
