using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Ticket.Context;
using Ticket.Helper;
using Ticket.Models;
using Ticket.ViewModel.Team;

namespace Ticket.Areas.Admin.Controllers;
[Area("Admin")]

public class TeamController : Controller
{
    private readonly AppDbContext _context;
    private readonly string _folderPath;
    private readonly IWebHostEnvironment _env;

    public TeamController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
        _folderPath = Path.Combine(_env.WebRootPath,"img");
    }

    public async Task<IActionResult> Index()
    {
        var team = await _context.Teams.Select(t => new TeamGetVM
        {
            Id = t.Id,
            FullName = t.FullName,
            Position = t.Position,
            SocialMedia = t.SocialMedia,
            Image = t.Image
        }).ToListAsync();
        return View(team);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TeamCreateVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        if (!vm.Image.CheckSize(2))
        {
            ModelState.AddModelError("Image", "Seklin olcusu 2mb-dan cox olmasin");
            return View(vm);
        }
        if (!vm.Image.CheckType("image"))
        {
            ModelState.AddModelError("Image", "Image tipli sekil yukleyin");
            return View(vm);
        }
        string uniqueFile = await vm.Image.FileUpload(_folderPath);

        Team team = new()
        {
            FullName = vm.FullName,
            Position=vm.Position,
            SocialMedia=vm.SocialMedia,
            Image=uniqueFile
        };
        await _context.Teams.AddAsync(team);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult>Delete(int id)
    {
        var team = await _context.Teams.FindAsync(id);
        if (team is null)
            return NotFound();
        _context.Teams.Remove(team);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var team = await _context.Teams.FindAsync(id);
        if(team is null)
            return NotFound();
        TeamUpdateVM vm = new()
        {
            FullName =  team.FullName,
            Position =  team.Position,
            SocialMedia =  team.SocialMedia
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(TeamUpdateVM vm)
    {
        if(!ModelState.IsValid)
            return View(vm);

        if (!vm.Image.CheckSize(2))
        {
            ModelState.AddModelError("Image","Seklin olcusu 2mb-dan cox olmaz");
            return View(vm);
        }

        if (!vm.Image.CheckType("image"))
        {
            ModelState.AddModelError("Image","Image tipli sekil yoxlayin");
            return View(vm);
        }

        var existTeam = await _context.Teams.FindAsync(vm.Id);
        if(existTeam is null)
            return BadRequest();

        existTeam.FullName = vm.FullName;
        existTeam.Position = vm.Position;
        existTeam.SocialMedia = vm.SocialMedia;
        if (vm.Image is { })
        {
            string newImagePath = await vm.Image.FileUpload(_folderPath);
            string oldImagePath = Path.Combine(_folderPath,existTeam.Image);
            ExtensionMethods.DeleteFile(oldImagePath);
            existTeam.Image=newImagePath;
        }
        
        _context.Teams.Update(existTeam);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
