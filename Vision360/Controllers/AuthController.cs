using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vision360.Context.Repositories.Interfaces;
using Vision360.Models;

namespace Vision360.Controllers;

public class AuthController(IAuthRepository repo) : Controller
{
    [HttpGet]
    public IActionResult Index(AuthViewModel model)
    {
        return View(model);
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        return View("Index", new AuthViewModel { IsRegistration = false, });
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(AuthViewModel model)
    {
        if (!ModelState.IsValid) return View("Index", model);

        var result = await repo.LoginAsync(model);
        
        if (result.Succeeded) 
            return RedirectToAction("Index", "Admin");

        ErrorResults(result);

        return View("Index", model);
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View("Index", new AuthViewModel { IsRegistration = true });
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(AuthViewModel model)
    {
        model.IsRegistration = true;
        
        if (!ModelState.IsValid) return View("Index", model);
        
        var result = await repo.RegisterAsync(model);

        if (result.Succeeded)
        {
            model.IsRegistration = false;
            return RedirectToAction("Index", model);
        }

        ErrorResults(result);
        
        return View("Index", model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult LogOut()
    {
        repo.LogoutAsync();
        
        return RedirectToAction("Login", "Auth");
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private void ErrorResults(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            switch (error.Code)
            {
                case "DuplicateEmail":
                    ModelState.AddModelError("Email", error.Description);
                    break;
                case "UserNotFound":
                    ModelState.AddModelError("Email", error.Description);
                    break;
                case "IsLockedOut":
                    ModelState.AddModelError("Email", error.Description);
                    break;
                case "PasswordTooShort":
                    ModelState.AddModelError("Password", error.Description);
                    break;
                case "InvalidPassword":
                    ModelState.AddModelError("Password", error.Description);
                    break;
                default:
                    ModelState.AddModelError(string.Empty, error.Description);
                    break;
            }
        }
    }
}