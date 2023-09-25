using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Repository.Model;
using Repository.Service;
using Repository.Tools;

namespace BookStoreAdvanced.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookGenreController : ControllerBase
    {
        private readonly IRepository<BookGenre> _bookGenreRepo;
        public BookGenreController(IMongoClient client)
        {
            _bookGenreRepo = new Repository<BookGenre>(client, "BookStoreDB","BookGenre");
        }
        [HttpPost]
        public async Task<IActionResult> addBookGender(BookGenreView genreView)
        {
            BookGenre bookGenre = new BookGenre
            {
                Id = IdGenerator.GenerateId(),
                Name = genreView.Name,
            };
            BookGenre genre = await _bookGenreRepo.addOneItem(bookGenre);
            return Ok(genre);
        }
    }
}
