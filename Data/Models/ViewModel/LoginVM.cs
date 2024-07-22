 using System.ComponentModel.DataAnnotations;
 public class LoginVM
    {
        [Required(ErrorMessage ="Username is required.")]
        public string? UserName {get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Password is required.")]
        public string? Password {get; set; }
    }