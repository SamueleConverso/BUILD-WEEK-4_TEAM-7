using System.IO.Pipelines;
using BUILD_WEEK_4_TEAM_7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BUILD_WEEK_4_TEAM_7.Controllers {
    public class CartController : Controller {
        private readonly string _connectionString;

        public CartController() {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet("product/cart")]
        public async Task<IActionResult> Index() {
            var cartList = new CartViewModel() {
                CartProducts = new List<Cart>()
            };

            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                string query = "SELECT * FROM Cart";

                await using (SqlCommand command = new SqlCommand(query, connection)) {
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync()) {
                        while (await reader.ReadAsync()) {
                            cartList.CartProducts.Add(
                                new Cart() {
                                    IdCart = reader.GetInt32(0),
                                    IdProduct = reader.GetGuid(1),
                                    ProductName = reader.GetString(2),
                                    Price = reader.GetDecimal(3),
                                    ImageURL = reader.GetString(4),
                                    Quantity = reader.GetInt32(5)
                                }
                            );
                        }
                    }
                }
            }
            return View(cartList);
        }

        [HttpGet("product/cart/remove-from-cart/{removeProductId:guid}")]
        public async Task<IActionResult> RemoveFromCart(Guid removeProductId) {
            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                string query = "DELETE FROM Cart WHERE IdProduct = @IdProduct";
                await using (SqlCommand command = new SqlCommand(query, connection)) {
                    command.Parameters.AddWithValue("@IdProduct", removeProductId);
                    int righeInteressate = await command.ExecuteNonQueryAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost("product/modify-quantity/{modifyQuantityIdProduct:guid}")]
        public async Task<IActionResult> ModifyQuantity(Cart cartProduct, Guid modifyQuantityIdProduct) {
            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                var query = "UPDATE CART SET QUANTITY = @Quantity WHERE IdProduct = @IdProduct";
                await using (SqlCommand command = new SqlCommand(query, connection)) {
                    command.Parameters.AddWithValue("@IdProduct", modifyQuantityIdProduct);
                    command.Parameters.AddWithValue("@Quantity", cartProduct.Quantity);
                    int righeInteressate = await command.ExecuteNonQueryAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet("product/cart/buy")]
        public async Task<IActionResult> Buy() {
            var cartlist = new CartViewModel() { CartProducts = new List<Cart>() };
            decimal total = 0;

            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                string query = "SELECT SUM(Price * Quantity) FROM Cart";
                await using (SqlCommand command = new SqlCommand(query, connection)) {
                    total = (decimal)await command.ExecuteScalarAsync();
                }
            }

            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                string query = "SELECT * FROM Cart";
                await using (SqlCommand command = new SqlCommand(query, connection))
                await using (SqlDataReader reader = await command.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {

                        cartlist.CartProducts.Add(new Cart() {
                            IdCart = reader.GetInt32(0),
                            IdProduct = reader.GetGuid(1),
                            ProductName = reader.GetString(2),
                            Price = reader.GetDecimal(3),
                            ImageURL = reader.GetString(4),
                            Quantity = reader.GetInt32(5)
                        }
                        );

                    }
                }
            }

            cartlist.TotalPrice = total;

            return View("Checkout", cartlist);
        }

        [HttpGet("product/cart/checkout")]
        public IActionResult Checkout(CartViewModel cart) {
            return View(cart);
        }

        [HttpGet("product/Checkout/{DeleteProductId:guid}")]
        public async Task<IActionResult> RemoveFromCheckOut(Guid DeleteProductId) {
            var cartlist = new CartViewModel() { CartProducts = new List<Cart>() };
            decimal total = 0;


            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                string query = "DELETE FROM Cart WHERE IdProduct = @IdProduct";
                await using (SqlCommand command = new SqlCommand(query, connection)) {
                    command.Parameters.AddWithValue("@IdProduct", DeleteProductId);
                    int righeInteressate = await command.ExecuteNonQueryAsync();
                }
            }

            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                string query = "SELECT * FROM Cart";
                await using (SqlCommand command = new SqlCommand(query, connection))
                await using (SqlDataReader reader = await command.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {

                        cartlist.CartProducts.Add(new Cart() {
                            IdCart = reader.GetInt32(0),
                            IdProduct = reader.GetGuid(1),
                            ProductName = reader.GetString(2),
                            Price = reader.GetDecimal(3),
                            ImageURL = reader.GetString(4),
                            Quantity = reader.GetInt32(5)
                        }
                        );

                    }
                }
            }

            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();
                string query = "SELECT SUM(Price * Quantity) FROM Cart";
                await using (SqlCommand command = new SqlCommand(query, connection)) {

                    var isNull = await command.ExecuteScalarAsync();
                    if (isNull != DBNull.Value) {
                        total = (decimal)isNull;
                    } else {
                        total = 0;
                    }
                }
            }

            cartlist.TotalPrice = total;

            return View("Checkout", cartlist);
        }
    }
}
