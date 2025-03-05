namespace BUILD_WEEK_4_TEAM_7.Models {
    public class CartTempProduct {
        public Guid IdProduct {
            get; set;
        }
        public string ProductName {
            get; set;
        }
        public decimal Price {
            get; set;
        }
        public string ImageURL {
            get; set;
        }

        public CartTempProduct(Guid idProduct, string productName, decimal price, string imageURL) {
            IdProduct = idProduct;
            ProductName = productName;
            Price = price;
            ImageURL = imageURL;
        }
    }
}
