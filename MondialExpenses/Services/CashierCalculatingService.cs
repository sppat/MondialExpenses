using Microsoft.EntityFrameworkCore;
using MondialExpenses.Data;
using MondialExpenses.Models;

namespace MondialExpenses.Services
{
    public class CashierCalculatingService
    {
        private readonly ApplicationDbContext _context;

        public CashierCalculatingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public double CalculateSum(Cashier cashier) => cashier.Card + cashier.Cash;
        public async Task<double> CalculateTotal(Cashier cashier)
        {
            var sum = CalculateSum(cashier);
            var expenses = await _context.Expenses
                .Where(e => e.CashierId == cashier.Id)
                .Select(e => e.Value)
                .ToListAsync();

            if (!expenses.Any())
            {
                return sum;
            }

            var total = sum - expenses.Aggregate((a, b) => a + b);

            return total;
        }
    }
}
