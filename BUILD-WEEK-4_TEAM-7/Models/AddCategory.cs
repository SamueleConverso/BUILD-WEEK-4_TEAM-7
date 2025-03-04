using System.ComponentModel.DataAnnotations;

namespace BUILD_WEEK_4_TEAM_7.Models {
    public class AddCategory {
        [Display(Name = "Nome categoria")]
        [Required(ErrorMessage = "Il nome è obbligatorio!")]
        [StringLength(50, ErrorMessage = "Il nome deve essere compreso tra 5 e 50 caratteri", MinimumLength = 5)]
        public string CategoryName {
            get; set;
        }
    }
}
