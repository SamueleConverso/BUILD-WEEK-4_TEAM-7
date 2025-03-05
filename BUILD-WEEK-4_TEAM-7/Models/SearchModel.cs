namespace BUILD_WEEK_4_TEAM_7.Models {
    public class SearchModel {
        public string? Query {
            get; set;
        }

        public List<Product>? SearchedProducts {
            get; set;
        } = new List<Product>();
    }
}
