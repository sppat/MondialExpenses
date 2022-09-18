using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MondialExpenses.Data;

namespace MondialExpenses.Controllers
{
    public class CashiersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CashiersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() => View(await _context.Cashiers.ToListAsync());

        public IActionResult Create() => View();
    }
}
