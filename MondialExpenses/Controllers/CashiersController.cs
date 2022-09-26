using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MondialExpenses.Data;
using MondialExpenses.Models;
using MondialExpenses.Services;
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

            return RedirectToAction("Create", "Expenses", new { Id = cashier.Id });
        }

        public async Task<IActionResult> Details(int id)
        {
            var cashier = await _context.Cashiers.Include(c => c.Expenses).FirstOrDefaultAsync(c => c.Id == id);
            if (cashier == null)
            {
                return View("NotFound");
            }

            return View(cashier);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cashier = await _context.Cashiers.Include(c => c.Expenses).FirstOrDefaultAsync(c => c.Id == id);
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

        public async Task<IActionResult> Export(int id)
        {
            var cashier = await _context.Cashiers
                .Include(c => c.Expenses)
                .FirstOrDefaultAsync(c => c.Id == id);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Cashier");
                worksheet.Range(4, 1, 4, 2).Merge();
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Day";
                worksheet.Cell(currentRow, 2).Value = "Card";
                worksheet.Cell(currentRow, 3).Value = "Cash";
                worksheet.Cell(currentRow, 4).Value = "Sum";
                worksheet.Cell(currentRow, 5).Value = "Total";

                currentRow++;

                worksheet.Cell(currentRow, 1).Value = cashier.Day.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 2).Value = string.Format("{0:c}", cashier.Card);
                worksheet.Cell(currentRow, 3).Value = string.Format("{0:c}", cashier.Cash);
                worksheet.Cell(currentRow, 4).Value = string.Format("{0:c}", cashier.Sum);
                worksheet.Cell(currentRow, 5).Value = string.Format("{0:c}", cashier.Total);

                currentRow += 2;
                worksheet.Cell(currentRow, 1).Value = "Expenses";
                foreach (var expense in cashier.Expenses)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = expense.Description;
                    worksheet.Cell(currentRow, 2).Value = string.Format("{0:c}", expense.Value);
                }

                worksheet.Column(1).AdjustToContents();
                worksheet.Row(1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Row(2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                worksheet.Row(4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Row(5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content, 
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                        $"cashier_{cashier.Day.ToString("dd/MM/yyyy")}.xlsx");
                }
            }
        }
    }
}
