using System.ComponentModel.DataAnnotations;

namespace BUILD_WEEK_4_TEAM_7.Models {
    public class LoginModel {
        [Required(ErrorMessage = "Il nome utente è obbligatorio")]
        public string Username {
            get; set;
        }

        [Required(ErrorMessage = "La password è obbligatoria")]
        [DataType(DataType.Password)]
        public string Password {
            get; set;
        }
    }
}