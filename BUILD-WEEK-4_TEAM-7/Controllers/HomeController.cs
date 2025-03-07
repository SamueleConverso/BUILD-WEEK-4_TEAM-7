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
            var categoryList = new CategoryViewModel() {
                Categories = new List<Category>()
            };

            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                string query = "SELECT IdProduct, ProductName, Description, Price, ImageURL, CategoryName FROM Products INNER JOIN Category on Category.IdCategory = Products.IdCategory WHERE Stock > 0";

                await using (SqlCommand command = new SqlCommand(query, connection)) {
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync()) {
                        while (await reader.ReadAsync()) {
                            productsList.Products.Add(
                                new Product() {
                                    IdProduct = reader.GetGuid(0),
                                    ProductName = reader.GetString(1),
                                    CategoryName = reader.GetString(5),
                                    Price = reader.GetDecimal(3),
                                    Description = reader.GetString(2),
                                    ImageURL = reader.GetString(4)
                                }
                            );
                        }
                    }
                }
            }
            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                string query = "SELECT * FROM Category";

                await using (SqlCommand command = new SqlCommand(query, connection)) {
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync()) {
                        while (await reader.ReadAsync()) {
                            categoryList.Categories.Add(
                                new Category() {
                                    IdCategory = reader.GetInt32(0),
                                    CategoryName = reader.GetString(1)
                                }
                            );
                        }
                    }
                }
            }

            ViewBag.Categories = categoryList.Categories;
            return View(productsList);
        }

        [HttpGet("product/add-to-cart/{cartProductId:guid}")]
        public async Task<IActionResult> AddToCart(Guid cartProductId) {
            CartTempProduct cartTempProduct = null;


            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                string query = "SELECT * FROM Products WHERE IdProduct = @IdProduct";

                await using (SqlCommand command = new SqlCommand(query, connection)) {
                    command.Parameters.AddWithValue("@IdProduct", cartProductId);
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync()) {
                        while (await reader.ReadAsync()) {
                            cartTempProduct = new CartTempProduct(reader.GetGuid(0), reader.GetString(1), reader.GetDecimal(4), reader.GetString(5));
                        }
                    }
                }
            }

            bool isAlreadyInCart = false;
            int productCount = 0;

            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                string query = "SELECT COUNT(*) FROM Cart WHERE IdProduct = @IdProduct";
                await using (SqlCommand command = new SqlCommand(query, connection)) {
                    command.Parameters.AddWithValue("@IdProduct", cartProductId);
                    productCount = (int)await command.ExecuteScalarAsync();
                }
            }

            if (productCount >= 1) {
                isAlreadyInCart = true;
            }

            if (!isAlreadyInCart) {
                await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                    await connection.OpenAsync();
                    string query = "INSERT INTO Cart VALUES (@IdProduct, @ProductName, @Price, @ImageURL, @Quantity)";
                    await using (SqlCommand command = new SqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@IdProduct", cartTempProduct!.IdProduct);
                        command.Parameters.AddWithValue("@ProductName", cartTempProduct.ProductName);
                        command.Parameters.AddWithValue("@Price", cartTempProduct.Price);
                        command.Parameters.AddWithValue("@ImageURL", cartTempProduct.ImageURL);
                        command.Parameters.AddWithValue("@Quantity", 1);
                        int righeInteressate = await command.ExecuteNonQueryAsync();
                    }
                }
            }
            return RedirectToAction("Index", "Cart");
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
