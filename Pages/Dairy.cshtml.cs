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
    public class Dairy : PageModel
    {
        private readonly ILogger<Dairy> _logger;


        private readonly IConfiguration _configuration;

        public Dairy(ILogger<Dairy> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public List<DairyList> dairylist = new List<DairyList>();
        public void OnGet()
        {
            try
            {
                string connectionString = _configuration["ConnectionStrings:defaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Connected");
                    string sql = "select * from Dairy";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DairyList data = new DairyList
                                {
                                    id = reader.GetInt32(0),
                                    productName = reader.GetString(1),
                                    price = reader.GetString(2),
                                    unit = reader.GetString(3),
                                    image = reader.IsDBNull(4) ? null : (byte[])reader["image"]
                                };
                                dairylist.Add(data);
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
    public class DairyList
    {
        public int id { get; set; }
        public string productName { get; set; } = "";
        public string price { get; set; } = "";
        public string unit { get; set; } = "";
        public byte[] image { get; set; }
    }
}