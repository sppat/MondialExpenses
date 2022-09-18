using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MondialExpenses.Data;
using MondialExpenses.Models;
using MondialExpenses.ViewModels;

namespace MondialExpenses.Controllers
{
    public class CashiersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CashiersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index() => View(await _context.Cashiers.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCashierVM createCashierVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createCashierVM);
            }

            var cashier = _mapper.Map<Cashier>(createCashierVM);

            _context.Cashiers.Add(cashier);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var cashier = await _context.Cashiers.FindAsync(id);
            if (cashier == null)
            {
                return View("NotFound");
            }

            return View(cashier);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cashier = await _context.Cashiers.FindAsync(id);
            if (cashier == null)
            {
                return View("NotFound");
            }

            var createCashierVM = _mapper.Map<CreateCashierVM>(cashier);

            return View(createCashierVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateCashierVM createCashierVM)
        {
            if(!ModelState.IsValid)
            {
                return View(createCashierVM);
            }

            var cashier = _mapper.Map<Cashier>(createCashierVM);

            _context.Cashiers.Update(cashier);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cashier = await _context.Cashiers.FindAsync(id);
            if (cashier == null)
            {
                return View("NotFound");
            }

            return View(cashier);
        }

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cashier = await _context.Cashiers.FindAsync(id);
            if (cashier == null)
            {
                return View("NotFound");
            }

            _context.Cashiers.Remove(cashier);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
