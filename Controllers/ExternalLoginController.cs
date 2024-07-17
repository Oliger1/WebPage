using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

[AllowAnonymous]
public class ExternalLoginController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public ExternalLoginController(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }


    [HttpGet]
    public IActionResult FacebookLogin(string returnUrl = "/")
    {
        var redirectUrl = Url.Action("FacebookResponse", "ExternalLogin", new { ReturnUrl = returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
        return Challenge(properties, "Facebook");
    }

    [HttpGet]
    public IActionResult GoogleLogin(string returnUrl = "/")
    {
        var redirectUrl = Url.Action("GoogleResponse", "ExternalLogin", new { ReturnUrl = returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        return Challenge(properties, "Google");
    }

    [HttpGet]
    public async Task<IActionResult> FacebookResponse(string returnUrl = "/")
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();

        if (info == null) 
        {
            return RedirectToAction("Login");
        }
        // Perform sign-in or sign-up logic
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            // Handle the case where no external login info is available
            return RedirectToAction("Login");
        }

        // Perform sign-in or sign-up logic

        return RedirectToAction("Index", "Home");
    }

}
