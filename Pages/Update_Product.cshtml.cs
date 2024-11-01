using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace OnlineShop.Pages
{
    public class Update_Product : PageModel
    {

        private readonly ILogger<Update_Product> _logger;

        private readonly IConfiguration _configuration;

        public Update_Product(ILogger<Update_Product> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }


        [BindProperty, Required(ErrorMessage = "Category is required")]
        public string category { get; set; } = "";
        [BindProperty, Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; } = "";
        [BindProperty, Required(ErrorMessage = "Price is required")]
        public string price { get; set; } = "";
        [BindProperty, Required(ErrorMessage = "Product unit is required")]
        public string unit { get; set; } = "";
        [BindProperty]
        public IFormFile productImage { get; set; }
        public byte[] image { get; set; }
        [BindProperty(SupportsGet = true)]
        public int p_id { get; set; }
        [BindProperty]
        public string tableNameH { get; set; }

        [BindProperty(SupportsGet = true)]
        public string product { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {

            try
            {
                Console.WriteLine($"{p_id} {product}");
                string tableName = null;
                string connectionString = _configuration["ConnectionStrings:defaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Connected Update");
                    string sql_table = @"select TableName from (select 'Candy' as TableName from Candy where productId=@ProductId and productName=@ProductName
                    union all 
                    SELECT 'Cooking' AS TableName FROM Cooking WHERE productId = @ProductId AND productName = @ProductName
                    UNION ALL
                    SELECT 'Dairy' AS TableName FROM Dairy WHERE productId = @ProductId AND productName = @ProductName
                    UNION ALL
                    SELECT 'Drinks' AS TableName FROM Drinks WHERE productId = @ProductId AND productName = @ProductName
                    UNION ALL
                    SELECT 'Fruits' AS TableName FROM Fruits WHERE productId = @ProductId AND productName = @ProductName
                    UNION ALL
                    SELECT 'Meat' AS TableName FROM Meat WHERE productId = @ProductId AND productName = @ProductName
                    UNION ALL
                    SELECT 'Sauces' AS TableName FROM Sauces WHERE productId = @ProductId AND productName = @ProductName
                    UNION ALL
                    SELECT 'Snacks' AS TableName FROM Snacks WHERE productId = @ProductId AND productName = @ProductName
                ) AS TableResult";
                    using (SqlCommand command = new SqlCommand(sql_table, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", p_id);
                        command.Parameters.AddWithValue("@ProductName", product);
                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            tableName = result.ToString();
                        }

                    }

                    string data = $"SELECT * FROM {tableName} WHERE productId=@productId AND productName=@productName";
                    using (SqlCommand command = new SqlCommand(data, connection))
                    {
                        command.Parameters.AddWithValue("@productId", p_id);
                        command.Parameters.AddWithValue("@productName", product);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            string tname = tableName;
                            tableNameH = tname;
                            if (tname == "Candy")
                            {
                                tname = "Candy & Chocolates";
                                Console.WriteLine(tname);
                            }
                            else if (tname == "Fruits")
                            {
                                tname = "Fruits & Vagetables";
                            }
                            else if (tname == "Meat")
                            {
                                tname = "Meat & Fish";
                            }
                            else if (tname == "Sauces")
                            {
                                tname = "Sauces & Pickles";
                            }

                            Console.WriteLine($"Price:{tname}");

                            if (reader.Read())
                            {

                                category = tname;
                                ProductName = reader.GetString(1);
                                price = reader.GetString(2);
                                unit = reader.GetString(3);
                            }
                        }
                    }

                }
            }
            catch (Exception e) { _logger.LogInformation(e.Message); };
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Console.WriteLine($"Table:{tableNameH}");
                byte[] byteImage = null;
                int i = 0;
                if (productImage != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await productImage.CopyToAsync(memoryStream);
                        byteImage = memoryStream.ToArray();
                        Console.WriteLine($"Image Byte Array Length: {byteImage.Length}");
                    }
                }
                else
                {
                    Console.WriteLine("No image uploaded.");
                    i++;
                }
                string connectionString = _configuration["ConnectionStrings:defaultConnection"];

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Connection Successful!");
                    string sql;
                    if (i > 0)
                    {
                        sql = $"update {tableNameH} set productName=@name,price=@price,unit=@unit where productId=@id";
                    }
                    else
                    {
                        sql = $"update {tableNameH} set productName=@name,price=@price,unit=@unit,image=@image where productId=@id";
                    }

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", ProductName);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@unit", unit);
                        command.Parameters.AddWithValue("@id", p_id);
                        if (i == 0)
                        {
                            command.Parameters.AddWithValue("@image", (object)byteImage ?? DBNull.Value);
                        }

                        await command.ExecuteNonQueryAsync();
                    }
                }

            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }
            return RedirectToPage("/ProductList", new { admin_auth = "mojahidaltarif78@gmail.com", product = "list" });
        }
    }
    public class UpdateList
    {
        public string category { get; set; } = "";
        public string productName { get; set; } = "";
        public string price { get; set; } = "";
        public string unit { get; set; } = "";
        public byte[] image { get; set; }
    }
}