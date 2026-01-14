using Microsoft.AspNetCore.Mvc;

namespace Ticket.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
