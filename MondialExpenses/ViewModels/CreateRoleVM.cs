using System.ComponentModel.DataAnnotations;

namespace MondialExpenses.ViewModels
{
    public class CreateRoleVM
    {
        [Required]
        public string Name { get; set; }
    }
}
