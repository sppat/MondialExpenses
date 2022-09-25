using MondialExpenses.Models;
using System.ComponentModel.DataAnnotations;

namespace MondialExpenses.ViewModels
{
    public class CreateCashierVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Cash amount is required.")]
        public double? Cash { get; set; }

        [Required(ErrorMessage = "Card amount is required.")]
        public double? Card { get; set; }

        [Required(ErrorMessage = "Day is required.")]
        public DateTime? Day { get; set; }

        public List<Expense> Expenses { get; set; }
    }
}
