using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authelia.Server.Interface.Pages
{
    public class LoginModel : PageModel
    {

        public IActionResult OnGet()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
               return RedirectToPage("/Privacy");
            } 

            return Page();
        }
    }
}
