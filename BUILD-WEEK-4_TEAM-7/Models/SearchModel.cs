namespace BUILD_WEEK_4_TEAM_7.Models {
    [Serializable]
    public class SearchModel {
        public string? Query {
            get; set;
        }

        public List<Product>? SearchedProducts {
            get; set;
        } = new List<Product>();

        public string Filter {
            get; set;
        }

        public List<Category> Categories {
            get; set;
        }
    }
}
