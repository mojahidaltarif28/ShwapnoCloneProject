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
        private readonly IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public List<FoodList> FruitList { get; set; } = new List<FoodList>();
        public List<FoodList> RecommandList { get; set; } = new List<FoodList>();
        public List<FoodList> Culninary { get; set; } = new List<FoodList>();
        public string ImageBase64 { get; set; } = "";
        public void OnGet()
        {
            try
            {
                string connectionString = _configuration["ConnectionStrings:defaultConnection"];


                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    string sql = "SELECT COALESCE(Fruits.productId, Meat.productId) AS productId, COALESCE(Fruits.productName, Meat.productName) AS productName,  COALESCE(Fruits.price, Meat.price) AS price, COALESCE(Fruits.unit, Meat.unit) AS unit,  COALESCE(Fruits.image, Meat.image) AS image FROM  Fruits FULL OUTER JOIN     Meat ON   Fruits.productName = Meat.productName;";
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
                            Random rng = new Random();
                            int n = FruitList.Count;
                            while (n > 1)
                            {
                                n--;
                                int k = rng.Next(n + 1);
                                FoodList value = FruitList[k];
                                FruitList[k] = FruitList[n];
                                FruitList[n] = value;
                            }

                        }
                    }

                }
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    string sqlrecommend = "SELECT COALESCE(Snacks.productId,Dairy.productId) AS productId, COALESCE(Snacks.productName,Dairy.productName) AS productName,  COALESCE(Snacks.price,Dairy.price) AS price, COALESCE(Snacks.unit,Dairy.unit) AS unit,  COALESCE(Snacks.image,Dairy.image) AS image FROM  Snacks FULL OUTER JOIN    Dairy ON   Snacks.productName = Dairy.productName;";
                    using (SqlCommand sqlCommand = new SqlCommand(sqlrecommend, sqlConnection))
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

                                RecommandList.Add(foodList);
                            }
                            Random rng = new Random();
                            int n = RecommandList.Count;
                            while (n > 1)
                            {
                                n--;
                                int k = rng.Next(n + 1);
                                FoodList value = RecommandList[k];
                                RecommandList[k] = RecommandList[n];
                                RecommandList[n] = value;
                            }

                        }
                    }
                }
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    string sqlrecommend = "SELECT * from Cooking";
                    using (SqlCommand sqlCommand = new SqlCommand(sqlrecommend, sqlConnection))
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

                                Culninary.Add(foodList);
                            }
                            Random rng = new Random();
                            int n = Culninary.Count;
                            while (n > 1)
                            {
                                n--;
                                int k = rng.Next(n + 1);
                                FoodList value = Culninary[k];
                                Culninary[k] = Culninary[n];
                                Culninary[n] = value;
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


