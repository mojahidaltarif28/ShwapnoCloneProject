using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace OnlineShop.Pages
{
    public class Cart : PageModel
    {
        private readonly ILogger<Cart> _logger;
        private readonly IConfiguration _configuration;

        public Cart(ILogger<Cart> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {

            string url = Request.Form["url"].ToString();
            if (string.IsNullOrEmpty(url)||url=="/")
            {
                url = "/Index"; 
            }

            try
            {
            
                string productName = Request.Form["productName"];
                string price = Request.Form["price"];
                string unit = Request.Form["unit"];
                string mobile = Request.Form["mobile"];
                string address = Request.Form["address"];
                Console.WriteLine("Name:" + productName + "\n price:" + int.Parse(price) + "\nunit:" + unit + "\nmobile:" + int.Parse(mobile) + "add:" + address);
                string connectionString = _configuration["ConnectionStrings:defaultConnection"];

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Connected Cart");

                    string sql = "insert into Cart (mobile,productName,price,unit,address) values (@mobile,@productName,@price,@unit,@address)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@mobile", int.Parse(mobile));
                        command.Parameters.AddWithValue("@productName", productName);
                        command.Parameters.AddWithValue("@price", int.Parse(price));
                        command.Parameters.AddWithValue("@unit", unit);
                        command.Parameters.AddWithValue("@address", address);
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