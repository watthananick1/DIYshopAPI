using Microsoft.AspNetCore.Mvc;

namespace DIYshopAPI.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
