using System.ComponentModel.DataAnnotations;

namespace MondialExpenses.ViewModels
{
    public class LoginVM
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
