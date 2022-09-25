using System.ComponentModel.DataAnnotations;

namespace MondialExpenses.Models
{
    public class Cashier
    {
        public int Id { get; set; }
        public double Cash { get; set; }
        public double Card { get; set; }
        public double Sum { get; set; }
        public double Total { get; set; }
        public DateTime Day { get; set; }
        public List<Expense> Expenses { get; set; }
    }
}
    