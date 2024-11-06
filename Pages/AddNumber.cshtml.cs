using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace OnlineShop.Pages
{
    public class AddNumber : PageModel
    {
        private readonly ILogger<AddNumber> _logger;
        private readonly IConfiguration _configuration;

        public AddNumber(ILogger<AddNumber> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        [BindProperty, Required(ErrorMessage = "Mobile number is required")]
        public int? mobile { get; set; }
        [BindProperty, Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = "";
        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string url = Request.Query["url"].ToString();
            if (url == "/")
            {
                url = "/Index";
            }
            try
            {
                string productName = Request.Query["productName"].ToString();
                string price = Request.Query["price"].ToString();
                string unit = Request.Query["unit"].ToString();

                price = price.Substring(1);
                Console.WriteLine(productName + "\nPrice:" + price + "\nUnit:" + unit);
                string connectionString = _configuration["ConnectionStrings:defaultConnection"];
                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append("Mobile", mobile.ToString(), options);
                Response.Cookies.Append("Address", Address, options);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "insert into Cart (mobile,productName,price,unit,address) values (@mobile,@productName,@price,@unit,@address)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@mobile", mobile);
                        command.Parameters.AddWithValue("@productName", productName);
                        command.Parameters.AddWithValue("@price", int.Parse(price));
                        command.Parameters.AddWithValue("@unit", unit);
                        command.Parameters.AddWithValue("@address", Address);
                        command.ExecuteNonQuery();
                    }

                }

            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }
            return RedirectToPage(url);


        }
    }


}