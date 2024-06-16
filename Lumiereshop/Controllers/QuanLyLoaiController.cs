using Microsoft.AspNetCore.Mvc;
using Lumiereshop.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Lumiereshop.Controllers
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class QuanLyLoaiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuanLyLoaiController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Loai.OrderByDescending(l => l.ID).ToListAsync());
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {              
                return NotFound();
            }

            var loai = await _context.Loai
                .FirstOrDefaultAsync(m => m.ID == id);
            if (loai == null)
            {
                return NotFound();
            }

            return View(loai);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Ten")] Loai loai)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loai);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm mới thành công!";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Thông tin không được để trống!";
            return View(loai);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loai = await _context.Loai.FindAsync(id);
            if (loai == null)
            {
                return NotFound();
            }
            return View(loai);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ten")] Loai loai)
        {
            if (id != loai.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loai);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiExists(loai.ID))
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
            return View(loai);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loai = await _context.Loai
                .FirstOrDefaultAsync(m => m.ID == id);
            if (loai == null)
            {
                return NotFound();
            }

            return View(loai);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var loai = await _context.Loai.FindAsync(id);
            _context.Loai.Remove(loai);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa thành công!";
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiExists(int id)
        {
            return _context.Loai.Any(e => e.ID == id);
        }
    }
}
