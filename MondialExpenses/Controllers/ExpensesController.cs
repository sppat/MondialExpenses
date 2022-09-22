using AutoMapper;
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
        private readonly IMapper _mapper;

        public ExpensesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<ExpenseVM> expensesVM)
        {
            if(!expensesVM.Any())
            {
                return RedirectToAction("Index", "Cashiers");
            }

            var expenses = _mapper.Map<List<Expense>>(expensesVM);
            
            _context.Expenses.AddRange(expenses);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Cashiers");
        }
    }
}
