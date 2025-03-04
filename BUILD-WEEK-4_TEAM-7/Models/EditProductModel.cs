using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace BUILD_WEEK_4_TEAM_7.Models {
    public class EditProductModel {
        public Guid IdProduct {
            get; set;
        }

        [Display(Name = "Product Name")]
        [StringLength(50, ErrorMessage = "Il nome deve essere compreso tra 5 e 50 caratteri", MinimumLength = 5)]
        public string? Name {
            get; set;
        }

        [Display(Name = "Short Description")]
        [StringLength(100, ErrorMessage = "La descrizione deve essere compresa tra 10 e 100 caratteri", MinimumLength = 10)]
        public string? Description {
            get; set;
        }

        [Display(Name = "Description")]
        [StringLength(600, ErrorMessage = "La descrizione deve essere compresa tra 10 e 600 caratteri", MinimumLength = 10)]
        public string? DescriptionExtra {
            get; set;
        }

        [Display(Name = "Price")]
        [Range(0.01, 1000, ErrorMessage = "Il prezzo deve essere compreso tra 0.01 e 1000")]
        public decimal? Price {
            get; set;
        }

        [Display(Name = "Image URL")]
        public string? ImageURL {
            get; set;
        }

        [Display(Name = "Stock")]
        public int? Stock {
            get; set;
        }

        [Display(Name = "Category")]
        public int? IdCategory {
            get; set;
        }

        public List<Category>? Categories {
            get; set;
        }
    }
}
