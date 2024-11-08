using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace OnlineShop.Pages
{
    public class Login : PageModel
    {
        private readonly ILogger<Login> _logger;

        public Login(ILogger<Login> logger)
        {
            _logger = logger;
        }
        [BindProperty,Required(ErrorMessage ="Email is required")]
        public string email {get;set;}
        [BindProperty,Required(ErrorMessage ="Password is required")]
        public string password{get;set;}
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            return RedirectToPage("/Admin",new{ admin_auth="mojahidaltarif78@gmail.com"});
        }
    }
}