using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace OnlineShop.Pages
{
    public class Fruits : PageModel
    {
        private readonly ILogger<Fruits> _logger;
        private readonly IConfiguration _configuration;

        public Fruits(ILogger<Fruits> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public List<FruitsList> fruitslist=new List<FruitsList>();
        public void OnGet()
        {
            try
            {
                string connectionString = _configuration["ConnectionStrings:defaultConnection"];
                using(SqlConnection connection=new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Connected");
                    string sql="select * from Fruits";
                    using(SqlCommand command=new SqlCommand(sql,connection))
                    {
                        using(SqlDataReader reader=command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                FruitsList data=new FruitsList{
                                    id=reader.GetInt32(0),
                                    productName=reader.GetString(1),
                                    price=reader.GetString(2),
                                    unit=reader.GetString(3),
                                    image=reader.IsDBNull(4)?null:(byte[])reader["image"]
                                };
                                fruitslist.Add(data);
                            }
                        }
                    }
                }
                
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while retrieving the connection string.");
            }
        }
    }
    public class FruitsList{
        public int id{get;set;}
        public string productName {get;set;}="";
        public string price {get;set;}="";
        public string unit{get;set;}="";
        public byte[] image{get;set;}
    }
}
