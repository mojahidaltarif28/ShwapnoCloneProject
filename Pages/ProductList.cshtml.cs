using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace OnlineShop.Pages
{
    public class ProductList : PageModel
    {
        private readonly ILogger<ProductList> _logger;
        private readonly IConfiguration _configuration;

        public ProductList(ILogger<ProductList> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public List<Food> foodlist = new List<Food>();
        public string imagebase { get; set; } = "";

        public void OnGet(string category,string search_item)
        {
            try
            {
                string cat_url="WHERE 1=1";
                if(!string.IsNullOrEmpty(search_item))
                {
                    cat_url+=" AND productName LIKE @category";
                }
                string connectionString = _configuration["ConnectionStrings:defaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Connected ProductList");
                    string sql=$"SELECT * FROM {category} {cat_url}";
                  
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@category",$"%{search_item}%");
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Food food = new Food
                                {
                                    id = reader.GetInt32(0),
                                    ProductName = reader.GetString(1),
                                    price = reader.GetString(2),
                                    unit = reader.GetString(3),
                                    image = reader.IsDBNull(4) ? null : (byte[])reader["image"]
                                };
                                foodlist.Add(food);
                            }
                        }
                    }
                }





            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);

            }
        }

    }
    public class Food
    {
        public int id { get; set; }
        public string ProductName { get; set; } = "";
        public string price { get; set; } = "";
        public string unit { get; set; } = "";
        public byte[] image { get; set; }

    }
}