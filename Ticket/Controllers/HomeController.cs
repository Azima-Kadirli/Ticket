using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ticket.Context;
using Ticket.ViewModel.Team;

namespace Ticket.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var team = await _db.Teams.Select(t => new TeamGetVM()
            {
                Id =  t.Id,
                FullName =  t.FullName,
                Position =   t.Position,
                SocialMedia =  t.SocialMedia,
                Image =  t.Image
            }).ToListAsync();
            return View(team);
        }
    }
}
