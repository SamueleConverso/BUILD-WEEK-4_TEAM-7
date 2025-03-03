using System.Diagnostics;
using BUILD_WEEK_4_TEAM_7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BUILD_WEEK_4_TEAM_7.Controllers {
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller {
        private readonly string _connectionString;

        public HomeController() {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IActionResult> Index() {
            var productsList = new ProductsViewModel() {
                Products = new List<Product>()
            };

            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                string query = "SELECT * FROM Products";

                await using (SqlCommand command = new SqlCommand(query, connection)) {
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync()) {
                        while (await reader.ReadAsync()) {
                            productsList.Products.Add(
                                new Product() {
                                    IdProduct = reader.GetGuid(0),
                                    ProductName = reader.GetString(1),
                                    //Category = reader.GetString(2),
                                    Price = reader.GetDecimal(4),
                                    Description = reader.GetString(2)
                                }
                            );
                        }
                    }
                }
            }

            return View(productsList);
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
