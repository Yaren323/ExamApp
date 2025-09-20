namespace ExamApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using ExamApp.Core.Entities;
using ExamApp.Service.Interfaces;

public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _userService.AuthenticateAsync(username, password);

        if (user == null)
        {
            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
            return View();
        }

        // Basit authentication (gerçek uygulamada JWT veya Identity kullanın)
        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("Username", user.Username);
        HttpContext.Session.SetString("Role", user.Role);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(User user, string password)
    {
        try
        {
            await _userService.CreateAsync(user, password);
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(user);
        }
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}