namespace MondialExpenses.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public int CashierId { get; set; }
        public Cashier Cashier { get; set; }
    }
}
