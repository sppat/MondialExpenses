using MondialExpenses.Models;

namespace MondialExpenses.ViewModels
{
    public class CreateExpensesVM
    {
        public Cashier Cashier { get; set; }
        public Expense Expense { get; set; }
    }
}
