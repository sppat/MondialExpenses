using System.ComponentModel.DataAnnotations;

namespace MondialExpenses.Models
{
    public class Cashier
    {
        public int Id { get; set; }

        [Required]
        public double Cash { get; set; }

        [Required]
        public double Card { get; set; }

        [Required]
        public DateTime Day { get; set; }

        [Required]
        public List<Expense> Expenses { get; set; }
    }
}
    