using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MVC.Views.Home
{
    public class LoginModel : PageModel
    {
        public string Username { get; internal set; }
        public string Password { get; internal set; }

        public void OnGet()
        {
        }
    }
}
