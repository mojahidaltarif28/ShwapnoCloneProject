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
    public class Meat : PageModel
    {
        private readonly ILogger<Meat> _logger;

         private readonly IConfiguration _configuration;

        public Meat(ILogger<Meat> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public List<MeatList> meatlist=new List<MeatList>();
        public void OnGet()
        {
            try
            {
                string connectionString = _configuration["ConnectionStrings:defaultConnection"];
                using(SqlConnection connection=new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Connected");
                    string sql="select * from Meat";
                    using(SqlCommand command=new SqlCommand(sql,connection))
                    {
                        using(SqlDataReader reader=command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                               MeatList data=new MeatList{
                                    id=reader.GetInt32(0),
                                    productName=reader.GetString(1),
                                    price=reader.GetString(2),
                                    unit=reader.GetString(3),
                                    image=reader.IsDBNull(4)?null:(byte[])reader["image"]
                                };
                                meatlist.Add(data);
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
    public class MeatList{
        public int id{get;set;}
        public string productName {get;set;}="";
        public string price {get;set;}="";
        public string unit{get;set;}="";
        public byte[] image{get;set;}
    }
}