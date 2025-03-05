using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BUILD_WEEK_4_TEAM_7.Models
{
    public class ProductDetailsModel
    {
        public Guid IdProduct { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Short Description")]
        public string Description { get; set; }

        [Display(Name = "Additional Description")]
        public string DescriptionExtra { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Image URL")]
        public string ImageURL { get; set; }

        [Display(Name = "Stock")]
        public int Stock { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La quantità deve essere almeno 1!")]
        public int Quantity { get; set; }
    }
}
