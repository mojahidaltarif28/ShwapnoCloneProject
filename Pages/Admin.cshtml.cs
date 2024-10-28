using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.Data.SqlClient;
using System.Drawing;
namespace OnlineShop.Pages
{
    public class Admin : PageModel
    {
        [BindProperty, Required(ErrorMessage = "Category is required")]
        public string category { get; set; } = "";
        [BindProperty, Required(ErrorMessage = "Product name is required")]
        public string productName { get; set; } = "";
        [BindProperty, Required(ErrorMessage = "Price is required")]
        public string price { get; set; } = "";
        [BindProperty, Required(ErrorMessage = "Product unit is required")]
        public string unit { get; set; } = "";
        [BindProperty, Required(ErrorMessage = "Product image is required")]

        public IFormFile productImage { get; set; }
        public string ErrorMessage { get; set; } = "";


        private readonly ILogger<Admin> _logger;

        public Admin(ILogger<Admin> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                byte[] byteImage = null;
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
        }
                string connectionString = "server=DESKTOP-65L1MDG\\SQLEXPRESS;database=Shwapno;Trusted_Connection=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Connection Successful!");
                    string sql = $"insert into {category} (productName,price,unit,image) values (@name,@price,@unit,@image)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", productName);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@unit", unit);
                        command.Parameters.AddWithValue("@image", (object)byteImage ?? DBNull.Value);
                       
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
            return RedirectToPage("/Admin",new{admin_auth="mojahidaltarif78@gmail.com",product="add"});
        }
    }
}