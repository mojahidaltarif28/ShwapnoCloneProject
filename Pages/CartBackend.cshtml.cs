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
    public class CartBackend : PageModel
    {
        private readonly ILogger<CartBackend> _logger;
        private readonly IConfiguration _configuration;

        private readonly CartService _cartService;
        public CartBackend(ILogger<CartBackend> logger, IConfiguration configuration,CartService cartService)
        {
            _logger = logger;
            _configuration = configuration;
            _cartService=cartService;
        }
        public List<CartItem> cartItems = new List<CartItem>();
        public int total_cart { get; set; }
        [BindProperty]
        public int id_delete { get; set; }
        public void OnGet()
        {
            try
            {
                string mobile = Request.Query["mobile"];
                string connectionString = _configuration["ConnectionStrings:defaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "select * from Cart where mobile=@mobile";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@mobile", mobile);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CartItem data = new CartItem
                                {
                                    id = reader.GetInt32(5),
                                    productName = reader.GetString(1),
                                    price = reader.GetInt32(2),
                                    unit = reader.GetString(3),
                                    address = reader.GetString(4)
                                };
                                cartItems.Add(data);
                            }
                        }
                    }

                    string sqlt = "select sum(price) from Cart where mobile=@mobile";
                    using (SqlCommand command1 = new SqlCommand(sqlt, connection))
                    {
                        command1.Parameters.AddWithValue("@mobile", mobile);
                        var result = command1.ExecuteScalar();
                        total_cart = Convert.ToInt32(result);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
            
        }
        public IActionResult OnPost()
        {
            string mobile = Request.Query["mobile"];
            try
            {

                string connectionString = _configuration["ConnectionStrings:defaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "delete from Cart where id=@id and mobile=@mobile";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id_delete);
                        command.Parameters.AddWithValue("@mobile", mobile);
                        command.ExecuteScalar();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }
            return RedirectToPage("/CartBackend", new { mobile = mobile, cart_mode = "cart_mode" });
        }

    }
    public class CartItem
    {
        public int id { get; set; }
        public string productName { get; set; } = "";
        public string address { get; set; } = "";
        public int price { get; set; }
        public string unit { get; set; }
    }
}