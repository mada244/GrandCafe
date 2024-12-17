using CoffeeShopClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace MongoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : MongoController<User>
    {
        public UserController() : base("users")
        {
        }
    }
}

