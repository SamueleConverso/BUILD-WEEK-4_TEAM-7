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
            string query = "";

            string newQueryString = searchModel.Query + "%";

            var searchList = new SearchModel() {
                SearchedProducts = new List<Product>()
            };

            searchModel.Categories = await GetCategories();

            foreach (var item in searchModel.Categories) {
                Console.WriteLine(item.CategoryName);
            }

            if (searchModel.Filter == "CategoryName ASC" || searchModel.Filter == "CategoryName DESC") {
                await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                    await connection.OpenAsync();
                    query = $"SELECT IdProduct, ProductName, Description, DescriptionExtra, Price, ImageURL, Stock, Products.IdCategory, CategoryName FROM Products INNER JOIN Category ON Products.IdCategory = Category.IdCategory WHERE ProductName LIKE @Query OR CategoryName LIKE @Query ORDER BY {searchModel.Filter}";
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
                                        CategoryName = reader.GetString(8)
                                    }
                                );
                            }
                        }
                    }
                }
            } else {
                await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                    await connection.OpenAsync();
                    query = $"SELECT IdProduct, ProductName, Description, DescriptionExtra, Price, ImageURL, Stock, Products.IdCategory, CategoryName FROM Products INNER JOIN Category ON Products.IdCategory = Category.IdCategory WHERE ProductName LIKE @Query OR CategoryName LIKE @Query ORDER BY {searchModel.Filter}";
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
                                        CategoryName = reader.GetString(8)
                                    }
                                );
                            }
                        }
                    }
                }
            }



            //var json = System.Text.Json.JsonSerializer.Serialize(searchList);
            //TempData["SearchList"] = json;

            return View("Index", searchList);
        }

        //[HttpPost("product/filter")]
        //public IActionResult ExecuteFilter(SearchModel searchModel) {
        //    Console.WriteLine(searchModel.Filter);

        //    var json = TempData["SearchList"] as string;
        //    var searchList = System.Text.Json.JsonSerializer.Deserialize<SearchModel>(json);

        //    Console.WriteLine(searchList!.SearchedProducts!.Count);

        //    return View("Index", searchList);
        //}


        private async Task<List<Category>> GetCategories() {
            List<Category> listaCategorie = new List<Category>();
            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                var query = "SELECT * FROM Category";

                await using (SqlCommand command = new SqlCommand(query, connection)) {
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync()) {

                        while (await reader.ReadAsync()) {
                            listaCategorie.Add(
                                new Category() {
                                    IdCategory = reader.GetInt32(0),
                                    CategoryName = reader.GetString(1)
                                }
                            );
                        }
                    }
                }
            }

            return listaCategorie;

        }

    }
}
