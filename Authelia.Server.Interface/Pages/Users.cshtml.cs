using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Authelia.Server.Authorization;

namespace Authelia.Server.Interface.Pages
{
    public class UsersModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (!User?.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }
    }
}
