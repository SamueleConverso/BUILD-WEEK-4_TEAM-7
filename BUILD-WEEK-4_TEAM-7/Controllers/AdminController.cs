using BUILD_WEEK_4_TEAM_7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace BUILD_WEEK_4_TEAM_7.Controllers {
    [AutoValidateAntiforgeryToken]
    public class AdminController : Controller {
        private readonly string _connectionString;

        public AdminController() {
            var configuration = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json", false, true)
                     .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Azione Index per visualizzare i prodotti
        [HttpGet("product/admin")]
        public async Task<IActionResult> Admin() {
            var productsList = new ProductsViewModel() {
                Products = new List<Product>()
            };

            using (SqlConnection conn = new SqlConnection(_connectionString)) {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Products", conn)) {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync()) {
                        while (await reader.ReadAsync()) {
                            productsList.Products.Add(new Product {
                                IdProduct = reader.GetGuid(0),
                                ProductName = reader.GetString(1),
                                Price = reader.GetDecimal(4),
                                Description = reader.GetString(2),
                                ImageURL = reader.GetString(5),
                                Stock = reader.GetInt32(6),
                                DescriptionExtra = reader.GetString(3),
                                Category = reader.GetInt32(7)
                            });
                        }
                    }
                }
            }
            ViewBag.Categories = await GetCategories();

            return View(productsList);
        }

        [HttpGet("product/add")]
        public async Task<IActionResult> AddProduct() {
            var model = new AddProductModel() {
                Categories = await GetCategories()
            };

            return View(model);
        }


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

        [HttpPost("product/create")]
        public async Task<IActionResult> CreateProduct(AddProductModel addProduct) {
            if (!ModelState.IsValid) {
                return RedirectToAction("AddProduct");
            }
            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                var query = "INSERT INTO Products (IdProduct, ProductName, Description, DescriptionExtra, Price, ImageURL, Stock, IdCategory) VALUES (@NEWID, @ProductName, @Description, @DescriptionExtra, @Price, @ImageURL, @Stock, @IdCategory)";
                await using (SqlCommand command = new SqlCommand(query, connection)) {
                    command.Parameters.AddWithValue("@NEWID", Guid.NewGuid());
                    command.Parameters.AddWithValue("@ProductName", addProduct.Name);
                    command.Parameters.AddWithValue("@Description", addProduct.Description);
                    command.Parameters.AddWithValue("@DescriptionExtra", addProduct.DescriptionExtra);
                    command.Parameters.AddWithValue("@Price", addProduct.Price);
                    command.Parameters.AddWithValue("@ImageURL", addProduct.ImageURL);
                    command.Parameters.AddWithValue("@Stock", addProduct.Stock);
                    command.Parameters.AddWithValue("@IdCategory", addProduct.IdCategory);

                    int righeInteressate = await command.ExecuteNonQueryAsync();
                }
            }
            return RedirectToAction("Admin");
        }


        [HttpGet("product/edit/{id:guid}")]
        public async Task<IActionResult> EditProduct(Guid id) {
            EditProductModel model = null;
            using (SqlConnection conn = new SqlConnection(_connectionString)) {
                await conn.OpenAsync();
                string query = "SELECT IdProduct, ProductName, Description, DescriptionExtra, Price, ImageURL, Stock, IdCategory FROM Products WHERE IdProduct = @IdProduct";
                using (SqlCommand cmd = new SqlCommand(query, conn)) {
                    cmd.Parameters.AddWithValue("@IdProduct", id);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync()) {
                        if (await reader.ReadAsync()) {
                            model = new EditProductModel {
                                IdProduct = reader.GetGuid(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                DescriptionExtra = reader.GetString(3),
                                Price = reader.GetDecimal(4),
                                ImageURL = reader.GetString(5),
                                Stock = reader.GetInt32(6),
                                IdCategory = reader.GetInt32(7),
                                Categories = await GetCategories()
                            };
                        }
                    }
                }
            }
            return View(model);
        }

        // Azione per aggiornare il prodotto nel database
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(EditProductModel editProduct) {
            if (!ModelState.IsValid) {
                editProduct.Categories = await GetCategories();
                return View("EditProduct", editProduct);
            }

            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                var query = "UPDATE Products SET ProductName = @ProductName, Description = @Description, DescriptionExtra = @DescriptionExtra, Price = @Price, ImageURL = @ImageURL, Stock = @Stock, IdCategory = @IdCategory WHERE IdProduct = @IdProduct";
                await using (SqlCommand command = new SqlCommand(query, connection)) {
                    command.Parameters.AddWithValue("@IdProduct", editProduct.IdProduct);
                    command.Parameters.AddWithValue("@ProductName", editProduct.Name);
                    command.Parameters.AddWithValue("@Description", editProduct.Description);
                    command.Parameters.AddWithValue("@DescriptionExtra", editProduct.DescriptionExtra);
                    command.Parameters.AddWithValue("@Price", editProduct.Price);
                    command.Parameters.AddWithValue("@ImageURL", editProduct.ImageURL);
                    command.Parameters.AddWithValue("@Stock", editProduct.Stock);
                    command.Parameters.AddWithValue("@IdCategory", editProduct.IdCategory);


                    int righeInteressate = await command.ExecuteNonQueryAsync();
                }
            }
            return RedirectToAction("Admin");
        }


        [HttpGet("product/delete/{id:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id) {
            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                var query = "DELETE FROM Products WHERE IdProduct = @IdProduct";
                await using (SqlCommand command = new SqlCommand(query, connection)) {
                    command.Parameters.AddWithValue("@IdProduct", id);

                    int righeInteressate = await command.ExecuteNonQueryAsync();
                }
            }
            return RedirectToAction("Admin");
        }


        [HttpGet("product/add-category")]
        public IActionResult AddCategory() {
            return View();
        }

        [HttpPost("product/create-category")]
        public async Task<IActionResult> CreateCategory(AddCategory category) {
            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                var query = "INSERT INTO Category (CategoryName) VALUES (@CategoryName)";
                await using (SqlCommand command = new SqlCommand(query, connection)) {
                    command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    int righeInteressate = await command.ExecuteNonQueryAsync();
                }
            }
            return RedirectToAction("Admin");
        }

        public async Task<IActionResult> Details(Guid id)
        {
            ProductDetailsModel product = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT p.IdProduct, p.ProductName, p.Description, p.DescriptionExtra, p.Price, p.ImageURL, p.Stock, c.CategoryName FROM Products p INNER JOIN Category c ON p.IdCategory = c.IdCategory WHERE IdProduct = @IdProduct";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdProduct", id);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            product = new ProductDetailsModel
                            {
                                IdProduct = reader.GetGuid(0),
                                ProductName = reader.GetString(1),
                                Description = reader.GetString(2),
                                DescriptionExtra = reader.GetString(3),
                                Price = reader.GetDecimal(4),
                                ImageURL = reader.GetString(5),
                                Stock = reader.GetInt32(6),
                                CategoryName = reader.GetString(7),
                                Quantity = 1 
                            };
                        }
                    }
                }
            }
            return View(product);
        }

    }
}




