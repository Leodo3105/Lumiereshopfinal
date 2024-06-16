using Microsoft.AspNetCore.Mvc;
using Lumiereshop.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Lumiereshop.Controllers
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class QuanLyLienHeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuanLyLienHeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.LienHe.OrderByDescending(l => l.ID).ToListAsync());
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lienHe = await _context.LienHe
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lienHe == null)
            {
                return NotFound();
            }

            return View(lienHe);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Ten,NoiDung")] LienHe lienHe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lienHe);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm mới thành công!";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Thông tin không được để trống!";
            return View(lienHe);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lienHe = await _context.LienHe.FindAsync(id);
            if (lienHe == null)
            {
                return NotFound();
            }
            return View(lienHe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ten,NoiDung")] LienHe lienHe)
        {
            if (id != lienHe.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lienHe);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LienHeExists(lienHe.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Thông tin không được để trống!";
            return View(lienHe);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lienHe = await _context.LienHe
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lienHe == null)
            {
                return NotFound();
            }

            return View(lienHe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var lienHe = await _context.LienHe.FindAsync(id);
            _context.LienHe.Remove(lienHe);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa thành công!";
            return RedirectToAction(nameof(Index));
        }

        private bool LienHeExists(int id)
        {
            return _context.LienHe.Any(e => e.ID == id);
        }
    }
}
