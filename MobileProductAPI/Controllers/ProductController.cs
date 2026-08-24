using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MobileProductAPI.Models;

namespace MobileProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            List<Product> products = new List<Product>();

            string connectionString =
                _configuration.GetConnectionString("DefaultConnection")
                ?? string.Empty;

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT
                        Id,
                        ProductName,
                        Price
                    FROM Products";

                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader =
                           command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                Id = Convert.ToInt32(reader["Id"]),

                                ProductName =
                                    reader["ProductName"].ToString()
                                    ?? string.Empty,

                                Price =
                                    Convert.ToDecimal(reader["Price"])
                            };

                            products.Add(product);
                        }
                    }
                }
            }

            return Ok(products);
        }
    }
}
