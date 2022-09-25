using System.ComponentModel.DataAnnotations;

namespace MondialExpenses.ViewModels
{
    public class ExpenseVM
    {
        public int CashierId { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
    }
}
