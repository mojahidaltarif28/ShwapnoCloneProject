using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace OnlineShop.Pages
{
    public class Delete_Product : PageModel
    {
        private readonly ILogger<Delete_Product> _logger;

        [BindProperty, Required]
        public string productId { get; set; } = "";
        [BindProperty, Required]
        public string productName { get; set; } = "";

        public void OnGet()
        {
        }
        public IActionResult OnPost()

        {
            if (!ModelState.IsValid)
            {

                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }
                return Page();
            }
            string tableName = null;
            string connectionString = "Server=DESKTOP-65L1MDG\\SQLEXPRESS;database=Shwapno;Trusted_Connection=True;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Connected Delete_Product");
                string sql_table = @"
                SELECT TableName FROM (
                    SELECT 'Candy' AS TableName FROM Candy WHERE productId = @ProductId AND productName = @ProductName
                    UNION ALL
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
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.Parameters.AddWithValue("@ProductName", productName);
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        tableName = result.ToString();
                    }
                    
                }
                

                string sql_delete = $"delete from {tableName} where productId=@productId";
                using (SqlCommand command = new SqlCommand(sql_delete, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    int row=command.ExecuteNonQuery();
                    Console.WriteLine($"{row} deleted from {tableName}");

                }
            }

            return RedirectToPage("/ProductList",new{admin_auth="mojahidaltarif78@gmail.com",product="list"});

        }
    }
}