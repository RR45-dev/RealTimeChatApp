using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

public class LoginModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public LoginModel(ApplicationDbContext context) => _context = context;

    [BindProperty]
    public string Username { get; set; }
    [BindProperty]
    public string Password { get; set; }

    public IActionResult OnPost()
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == Username && u.PasswordHash == Password);
        if (user == null) return Page();

        var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username) };
        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);
        HttpContext.SignInAsync("Cookies", principal);

        return RedirectToPage("/Chat");
    }
}
