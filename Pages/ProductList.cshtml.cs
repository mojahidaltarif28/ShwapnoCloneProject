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

        public ProductList(ILogger<ProductList> logger)
        {
            _logger = logger;
        }
        public List<Food> foodlist = new List<Food>();
        public string imagebase { get; set; } = "";

        public void OnGet()
        {
            try
            {
                string connectionString = "Server=DESKTOP-65L1MDG\\SQLEXPRESS;Database=Shwapno;Trusted_Connection=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Connected");
                    string sql = "select coalesce(Candy.productId,Cooking.productId,Dairy.productId,Drinks.productId,Fruits.productId,Meat.productId,Sauces.productId,Snacks.productId) as productId, coalesce(Candy.productName,Cooking.productName,Dairy.productName,"
                    + "Drinks.productName,Fruits.productName,Meat.productName,Sauces.productName,Snacks.productName) as productName, coalesce(Candy.price,Cooking.price,Dairy.price,Drinks.price,Fruits.price,Meat.price,Sauces.price,Snacks.price) as price,"
                    + "coalesce(Candy.unit,Cooking.unit,Dairy.unit,Drinks.unit,Fruits.unit,Meat.unit,Sauces.unit,Snacks.unit) as unit,coalesce(Candy.image,Cooking.image,Dairy.image,Drinks.image,Fruits.image,Meat.image,Sauces.image,Snacks.image) as image from Candy "
                    + "full outer join Cooking on Cooking.productName=Candy.productName "
                    + "full outer join  Dairy on coalesce(Cooking.productName,Candy.productName)=Dairy.productName "
                    + "full outer join Drinks on coalesce(Cooking.productName,Candy.productName,Dairy.productName)=Drinks.productName "
                    + "full outer join Fruits on coalesce(Cooking.productName,Candy.productName,Dairy.productName,Drinks.productName)=Fruits.productName "
                    + "full outer join Meat on coalesce(Cooking.productName,Candy.productName,Dairy.productName,Drinks.productName,Fruits.productName)=Meat.productName "
                    + "full outer join Sauces on coalesce(Cooking.productName,Candy.productName,Dairy.productName,Drinks.productName,Fruits.productName,Meat.productName)=Sauces.productName "
                    + "full outer join Snacks on coalesce(Cooking.productName,Candy.productName,Dairy.productName,Drinks.productName,Fruits.productName,Meat.productName,Sauces.productName)=Snacks.productName;";
                    using(SqlCommand command=new SqlCommand(sql,connection))
                    {
                        using(SqlDataReader reader=command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                Food food=new Food{
                                    id=reader.GetInt32(0),
                                    ProductName=reader.GetString(1),
                                    price=reader.GetString(2),
                                    unit=reader.GetString(3),
                                    image=reader.IsDBNull(4)?null:(byte[])reader["image"]
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