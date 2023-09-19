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
    public class MerchandiseController : ControllerBase, IMerchandiseController
    {
        public IRepository<Merchandise> _merchandiseRepos;
        public MerchandiseController(IMongoClient client)
        {
            _merchandiseRepos = new Repository<Merchandise>(client, "BookStoreDB", "Merchandise");
        }
    }
}
