using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MondialExpenses.Data;
using MondialExpenses.Models;
using MondialExpenses.ViewModels;

namespace MondialExpenses.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() => View(await _context.Expenses.ToListAsync());

        public async Task<IActionResult> Create(int id)
        {
            var cashier = await _context.Cashiers.FindAsync(id);
            if (cashier == null)
            {
                return View("NotFound");
            }

            var createExpensesVM = new CreateExpensesVM()
            {
                Cashier = cashier
            };

            return View(createExpensesVM);
        }
    }
}
