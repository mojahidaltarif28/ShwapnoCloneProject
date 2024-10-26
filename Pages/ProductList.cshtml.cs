using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace OnlineShop.Pages
{
    public class ProductList : PageModel
    {
        private readonly ILogger<ProductList> _logger;

        public ProductList(ILogger<ProductList> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}