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
    public class Search : PageModel
    {
        private readonly ILogger<Search> _logger;
        private readonly IConfiguration _configuration;


        public Search(ILogger<Search> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public List<SearchItem> searchProduct = new List<SearchItem>();

        public void OnGet()
        {
            try
            {
                string item_name = "%"+Request.Query["search_value"]+"%";
                string ConnectionStrings = _configuration["ConnectionStrings:defaultConnection"];
                using (SqlConnection connection = new SqlConnection(ConnectionStrings))
                {
                    connection.Open();
           
                    string sql = "SELECT *FROM (SELECT  CASE"
                            + " WHEN Candy.productId IS NOT NULL THEN 'Candy'"
                            + " WHEN Cooking.productId IS NOT NULL THEN 'Cooking'"
                            + " WHEN Dairy.productId IS NOT NULL THEN 'Dairy'"
                            + " WHEN Drinks.productId IS NOT NULL THEN 'Drinks'"
                            + " WHEN Fruits.productId IS NOT NULL THEN 'Fruits'"
                            + " WHEN Meat.productId IS NOT NULL THEN 'Meat'"
                            + " WHEN Sauces.productId IS NOT NULL THEN 'Sauces'"
                            + " WHEN Snacks.productId IS NOT NULL THEN 'Snacks'"
                            + " END AS TableName,"
                            + " COALESCE(Candy.productId, Cooking.productId, Dairy.productId, Drinks.productId, Fruits.productId, Meat.productId, Sauces.productId, Snacks.productId) AS productId,"
                            + " COALESCE(Candy.productName, Cooking.productName, Dairy.productName, Drinks.productName, Fruits.productName, Meat.productName, Sauces.productName, Snacks.productName) AS productName,"
                            + " COALESCE(Candy.price, Cooking.price, Dairy.price, Drinks.price, Fruits.price, Meat.price, Sauces.price, Snacks.price) AS price,"
                            + " COALESCE(Candy.unit, Cooking.unit, Dairy.unit, Drinks.unit, Fruits.unit, Meat.unit, Sauces.unit, Snacks.unit) AS unit,"
                            + " COALESCE(Candy.image, Cooking.image, Dairy.image, Drinks.image, Fruits.image, Meat.image, Sauces.image, Snacks.image) AS image"
                            + " FROM Candy "
                            + " FULL OUTER JOIN Cooking ON Cooking.productName = Candy.productName"
                            + " FULL OUTER JOIN Dairy ON COALESCE(Cooking.productName, Candy.productName) = Dairy.productName"
                            + " FULL OUTER JOIN Drinks ON COALESCE(Cooking.productName, Candy.productName, Dairy.productName) = Drinks.productName"
                            + " FULL OUTER JOIN Fruits ON COALESCE(Cooking.productName, Candy.productName, Dairy.productName, Drinks.productName) = Fruits.productName"
                            + " FULL OUTER JOIN Meat ON COALESCE(Cooking.productName, Candy.productName, Dairy.productName, Drinks.productName, Fruits.productName) = Meat.productName"
                            + " FULL OUTER JOIN Sauces ON COALESCE(Cooking.productName, Candy.productName, Dairy.productName, Drinks.productName, Fruits.productName, Meat.productName) = Sauces.productName"
                            + " FULL OUTER JOIN Snacks ON COALESCE(Cooking.productName, Candy.productName, Dairy.productName, Drinks.productName, Fruits.productName, Meat.productName, Sauces.productName) = Snacks.productName"
                            + ") AS Products "
                            + "where ("
                            + " TableName LIKE @searchItem OR productName LIKE @searchProduct);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@searchItem", item_name);
                        command.Parameters.AddWithValue("@searchProduct", item_name);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SearchItem searchItem = new SearchItem
                                {
                                    id = reader.GetInt32(1),
                                    ProductName = reader.GetString(2),
                                    price = reader.GetString(3),
                                    unit = reader.GetString(4),
                                    image = reader.IsDBNull(5) ? null : (byte[])reader["image"]
                                };
                                searchProduct.Add(searchItem);
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
public class SearchItem
{
    public int id { get; set; }
    public string ProductName { get; set; } = "";
    public string price { get; set; } = "";
    public string unit { get; set; } = "";
    public byte[] image { get; set; }

}
}
