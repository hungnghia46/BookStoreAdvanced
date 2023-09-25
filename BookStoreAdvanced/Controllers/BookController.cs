using BookStoreAdvanced.IController;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Repository.Model;
using Repository.ModelView;
using Repository.Service;
using Repository.Tools;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStoreAdvanced.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase, IBookController
    {
        private readonly ILogger<BookController> _logger;
        private readonly IRepository<Book> _bookRepos;
        private readonly IRepository<BookGenre> _genreRepos;
        public BookController(ILogger<BookController> logger, IMongoClient client)
        {
            _logger = logger;
            _bookRepos = new Repository<Book>(client, "BookStoreDB", "Books");
            _genreRepos = new Repository<BookGenre>(client, "BookStoreDB", "BookGenre");
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
        public async Task<IActionResult> GetBookById([FromQuery][Required] String id)
        {
            Expression<Func<Book, bool>> filterExpresstion = x => x.Id == id;
            IEnumerable<Book> filtedBookList = await _bookRepos.GetsByFilterAsync(filterExpresstion);
            if (filtedBookList.Any())
            {
                _logger.LogInformation($"Book:{filtedBookList.First().Title} retrieved");
                return Ok(filtedBookList);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
        }
        /// <summary>
        /// Get a book by it title
        /// </summary>
        /// <param name="title">The title of book to search</param>
        /// <returns>Book</returns>
        [HttpGet("Get-Book-By-Title")]
        public async Task<IActionResult> GetBookByName([FromQuery][Required] string title)
        {
            Expression<Func<Book, bool>> filterExpression = x => x.Title == title;
            IEnumerable<Book> filtedBookList = await _bookRepos.GetsByFilterAsync(filterExpression);
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
            try
            {
                Expression<Func<BookGenre, bool>> filterExpression = x => x.Name == bookView.Genre;
                IEnumerable<BookGenre> bookGenres = await _genreRepos.GetsByFilterAsync(filterExpression);
                BookGenre bookGenre = bookGenres.First();
            }catch (Exception) {
                return UnprocessableEntity($"Genre: {bookView.Genre} not valid!");
            }
            Book book = new Book
            {
                Id = IdGenerator.GenerateId(),
                Author = bookView.Author,
                Description = bookView.Description,
                BookGenre = bookView.Genre,
                ImageUrl = bookView.ImageUrl,
                InventoryQuantity = bookView.InventoryQuantity,
                Price = bookView.Price,
                ISBN = bookView.ISBN,
                PublicationYear = bookView.PublicationYear,
                Title = bookView.Title,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            await _bookRepos.addOneItem(book);
            return Ok(book);
        }
        /// <summary>
        /// Add A list of book
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
                    Id = IdGenerator.GenerateId(),
                    Title = item.Title,
                    Author = item.Author,
                    Description = item.Description,
                    BookGenre = item.Genre,
                    ImageUrl = item.ImageUrl,
                    InventoryQuantity = item.InventoryQuantity,
                    ISBN = item.ISBN,
                    Price = item.Price,
                    PublicationYear = item.PublicationYear,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            var bookAddedList = await _bookRepos.addManyItem(bookList);
            if (bookAddedList.Count > 0)
            {
                _logger.LogInformation($"{bookAddedList.Count} added!");
                return Ok(bookList);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);

        }
        /// <summary>
        /// update A book base on it Id
        /// </summary>
        /// <param name="ID">Id of book update</param>
        /// <param name="bookView"></param>
        /// <returns>Updated book</returns>

        [HttpPut("Update-Book-By-Id")]
        public async Task<IActionResult> UpdateBookById([FromQuery][Required] String ID, BookView bookView)
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
                .Set("price", bookView.Price)
                .Set("updatedAt", DateTime.UtcNow);

            Book updatedBook = await _bookRepos.updateItemByValue(ID, update);
            if (updatedBook != null)
            {
                _logger.LogInformation($"Book: {updatedBook.Id} is updated!");
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
        public async Task<IActionResult> DeleteBookById([FromQuery][Required] String id)
        {
            bool isDelete = await _bookRepos.removeItemByValue(id);
            if (isDelete)
            {
                _logger.LogInformation($"Book:{id} is deleted!");
                return Ok($"Book id: {id} is deleted!");
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
