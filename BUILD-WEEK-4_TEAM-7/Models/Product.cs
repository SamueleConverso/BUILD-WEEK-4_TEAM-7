namespace BUILD_WEEK_4_TEAM_7.Models {
    [Serializable]
    public class Product {

        public Guid IdProduct {
            get; set;
        }

        public string ProductName {
            get; set;
        }

        public string Description {
            get; set;
        }

        public string DescriptionExtra {
            get; set;
        }

        public decimal Price {
            get; set;
        }

        public string ImageURL {
            get; set;
        }

        public int Stock {
            get; set;
        }

        public int Category {
            get; set;
        }

        public string CategoryName {
            get; set;
        }

        public int Quantity {
            get; set;
        }
    }
}
