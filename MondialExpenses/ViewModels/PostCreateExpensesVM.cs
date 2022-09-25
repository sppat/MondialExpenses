using MondialExpenses.Models;

namespace MondialExpenses.ViewModels
{
    public class PostCreateExpensesVM
    {
        public int CashierId { get; set; }
        public List<ExpenseVM> ExpensesVM { get; set; }
    }
}
