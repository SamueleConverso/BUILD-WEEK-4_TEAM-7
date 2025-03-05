using BUILD_WEEK_4_TEAM_7.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BUILD_WEEK_4_TEAM_7.Controllers {
    public class SearchController : Controller {
        private readonly string _connectionString;

        public SearchController() {
            var configuration = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json", false, true)
                     .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        [HttpGet("product/search")]
        public IActionResult Index(SearchModel searchModel) {
            return View(searchModel);
        }

        [HttpPost("product/searching")]
        public async Task<IActionResult> ExecuteQuery(SearchModel searchModel) {
            string newQueryString = searchModel.Query + "%";

            var searchList = new SearchModel() {
                SearchedProducts = new List<Product>()
            };

            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                string query = "SELECT * FROM Products WHERE ProductName LIKE @Query";
                await using (SqlCommand command = new SqlCommand(query, connection)) {
                    command.Parameters.AddWithValue("@Query", newQueryString);
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync()) {
                        while (await reader.ReadAsync()) {
                            searchList.SearchedProducts.Add(
                                new Product() {
                                    IdProduct = reader.GetGuid(0),
                                    ProductName = reader.GetString(1),
                                    Description = reader.GetString(2),
                                    DescriptionExtra = reader.GetString(3),
                                    Price = reader.GetDecimal(4),
                                    ImageURL = reader.GetString(5),
                                    Stock = reader.GetInt32(6),
                                    Category = reader.GetInt32(7),
                                }
                            );
                        }
                    }
                }
            }

            foreach (var prod in searchList.SearchedProducts) {
                Console.WriteLine(prod.ProductName);
            }

            return View("Index", searchList);
        }
    }
}
