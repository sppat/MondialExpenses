using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MondialExpenses.Data;
using MondialExpenses.Models;
using MondialExpenses.Services;
using MondialExpenses.ViewModels;

namespace MondialExpenses.Controllers
{
    [Authorize]
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CashierCalculatingService _cashierCalculatingService;
        private readonly IMapper _mapper;

        public ExpensesController(ApplicationDbContext context, CashierCalculatingService cashierCalculatingService, IMapper mapper)
        {
            _context = context;
            _cashierCalculatingService = cashierCalculatingService;
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

            var createExpensesVM = new GetCreateExpensesVM()
            {
                Cashier = cashier
            };

            return View(createExpensesVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostCreateExpensesVM postCreateExpensesVM)
        {
            var cashier = await _context.Cashiers.FindAsync(postCreateExpensesVM.CashierId);
            var sum = _cashierCalculatingService.CalculateSum(cashier);

            if (postCreateExpensesVM.ExpensesVM == null)
            {
                cashier.Sum = sum;
                cashier.Total = sum;
                
                _context.Cashiers.Update(cashier);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Cashiers");
            }

            var expenses = _mapper.Map<List<Expense>>(postCreateExpensesVM.ExpensesVM);
            
            _context.Expenses.AddRange(expenses);
            await _context.SaveChangesAsync();

            cashier.Sum = sum;
            cashier.Total = await _cashierCalculatingService.CalculateTotal(cashier);

            _context.Cashiers.Update(cashier);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Cashiers");
        }
    }
}
