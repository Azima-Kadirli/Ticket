using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ticket.Models;
using Ticket.ViewModel.User;

namespace Ticket.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> userManager;
    private readonly SignInManager<AppUser> signInManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        AppUser user = new()
        {
            FullName = vm.Fullname,
            Email = vm.Email,
            UserName = vm.Username,
        };
       var result =  await userManager.CreateAsync(user, vm.Password);
       if (!result.Succeeded)
       {
           foreach (var error in result.Errors)
           {
               ModelState.AddModelError("", error.Description);
           }
       }
        await userManager.AddToRoleAsync(user,"Member");
        await signInManager.SignInAsync(user, false);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Login()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login(LoginVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);
        var user = await userManager.FindByEmailAsync(vm.Email);
        if (user == null)
        {
            ModelState.AddModelError("", "Email or password is incorrect.");
            return View(vm);
        }
        var result = await signInManager.PasswordSignInAsync(user, vm.Password, false, true);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Email or password is incorrect.");
            return View(vm);
        }
        return  RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    
    
    // public async Task<IActionResult> CreateRoles()
    // {
    //     await roleManager.CreateAsync(new IdentityRole()
    //     {
    //         Name = "Admin"
    //     });
    //     await roleManager.CreateAsync(new IdentityRole()
    //     {
    //         Name = "Member"
    //     });
    //     return Ok("Roles created");
    // }
}