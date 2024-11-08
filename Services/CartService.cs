using Microsoft.Data.SqlClient;

public class CartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    public CartService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }
    public int CartCount { get; set; }

    public void SetCartCount()
    {
        string mobile = _httpContextAccessor.HttpContext.Request.Cookies["mobile"] ?? string.Empty;
        if (string.IsNullOrEmpty(mobile))
        {
            CartCount = 0;
            return;
        }
        try
        {
            string connectionString = _configuration["ConnectionStrings:defaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "select count(id) from Cart where mobile=@mobile";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@mobile", int.Parse(mobile));
                    var result = command.ExecuteScalar();
                    CartCount = Convert.ToInt32(result);
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}