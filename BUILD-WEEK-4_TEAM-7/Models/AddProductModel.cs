using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace BUILD_WEEK_4_TEAM_7.Models
{
    public class AddProductModel
    {
        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Il nome è obbligatorio!")]
        [StringLength(50, ErrorMessage = "Il nome deve essere compreso tra 5 e 50 caratteri", MinimumLength = 5)]
        public string? Name { get; set; }

        [Display(Name = "Short Description")]
        [Required(ErrorMessage = "La descrizione è obbligatoria!")]
        [StringLength(100, ErrorMessage = "La descrizione deve essere compresa tra 10 e 100 caratteri", MinimumLength = 10)]
        public string? Description { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "La descrizione è obbligatoria!")]
        [StringLength(600, ErrorMessage = "La descrizione deve essere compresa tra 10 e 600 caratteri", MinimumLength = 10)]
        public string? DescriptionExtra { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "Il prezzo è obbligatorio!")]
        [Range(0.01, 1000, ErrorMessage = "Il prezzo deve essere compreso tra 0.01 e 1000")]
        public decimal? Price { get; set; }

        [Display(Name = "Image URL")]
        [Required(ErrorMessage = "L'URL dell'immagine è obbligatorio!")]
        public string? ImageURL { get; set; }

        [Display(Name = "Stock")]
        [Required(ErrorMessage = "La quantità è obbligatoria!")]
        public int? Stock { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "La categoria è obbligatoria!")]
        public int? IdCategory { get; set; }

        public List<Category>? Categories { get; set; }
    }
}
