using System.ComponentModel.DataAnnotations;

namespace MondialExpenses.ViewModels
{
    public class ExpenseVM
    {
        public string? Description { get; set; }
        public double? Value { get; set; }
        public int CashierId { get; set; }
    }
}
