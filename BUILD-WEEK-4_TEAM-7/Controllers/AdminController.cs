using BUILD_WEEK_4_TEAM_7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace BUILD_WEEK_4_TEAM_7.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AdminController : Controller
    {
        private readonly string _connectionString;

        public AdminController()
        {
            var configuration = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json", false, true)
                     .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Azione Index per visualizzare i prodotti
        public async Task<IActionResult> Admin()
        {
            var productsList = new ProductsViewModel()
            {
                Products = new List<Product>()
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Products", conn))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            productsList.Products.Add(new Product
                            {
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

            return View(productsList);
        }

        public async Task<IActionResult> AddProduct()
        {
            var model = new AddProductModel()
            {
                Categories = await GetCategories()
            };

            return View(model);
        }


        private async Task<List<Category>> GetCategories()
        {
            List<Category> listaCategorie = new List<Category>();
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Category";

                await using (SqlCommand command = new SqlCommand(query, connection))
                {
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            listaCategorie.Add(
                                new Category()
                                {
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

        [HttpPost]
        public async Task<IActionResult> CreateProduct(AddProductModel addProduct)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddProduct");
            }
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "INSERT INTO Products (IdProduct, ProductName, Description, DescriptionExtra, Price, ImageURL, Stock, IdCategory) VALUES (@NEWID, @ProductName, @Description, @DescriptionExtra, @Price, @ImageURL, @Stock, @IdCategory)";
                await using (SqlCommand command = new SqlCommand(query, connection))
                {
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

    }


}

