using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace OnlineShop.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<FoodList> FruitList { get; set; } = new List<FoodList>();
        public string ImageBase64 { get; set; } = "";
        public void OnGet()
        {
            try
            {
                string connectionString = "Server=DESKTOP-65L1MDG\\SQLEXPRESS;Database=Shwapno;Trusted_Connection=True;TrustServerCertificate=True";


                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    string sql = "select * from Fruits";
                    using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                    {
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FoodList foodList = new FoodList
                                {
                                    id = reader.GetInt32(0),
                                    productName = reader.GetString(1),
                                    price = reader.GetString(2),
                                    unit = reader.GetString(3),
                                    image = reader.IsDBNull(4) ? null : (byte[])reader["image"]
                                };
                                FruitList.Add(foodList);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
            }
        }
    }

    public class FoodList
    {
        public int id { get; set; }
        public string productName { get; set; } = "";
        public string price { get; set; } = "";
        public string unit { get; set; } = "";
        public byte[] image { get; set; }
    }
}


