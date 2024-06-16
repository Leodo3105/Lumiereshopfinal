using Microsoft.AspNetCore.Mvc;
using Lumiereshop.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Lumiereshop.Controllers
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class QuanLyNguoiDungController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuanLyNguoiDungController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.NguoiDung.OrderByDescending(nd => nd.ID).ToListAsync());
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDung
                .FirstOrDefaultAsync(m => m.ID == id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            return View(nguoiDung);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TaiKhoan,MatKhau,Ten,DiaChi,Quyen")] NguoiDung nguoiDung)
        {
            // Kiểm tra xem tài khoản đã tồn tại trong CSDL hay chưa
            var existingUser = await _context.NguoiDung.FirstOrDefaultAsync(u => u.TaiKhoan == nguoiDung.TaiKhoan);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "Tài khoản đã tồn tại.";
                return View(nguoiDung);
            }

            if (ModelState.IsValid)
            {
                _context.Add(nguoiDung);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm mới thành công!";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Thông tin không được để trống!";
            return View(nguoiDung);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDung.FindAsync(id);
            if (nguoiDung == null)
            {
                return NotFound();
            }
            return View(nguoiDung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TaiKhoan,MatKhau,Ten,DiaChi,Quyen")] NguoiDung nguoiDung)
        {
            if (id != nguoiDung.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nguoiDung);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NguoiDungExists(nguoiDung.ID))
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
            return View(nguoiDung);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDung
                .FirstOrDefaultAsync(m => m.ID == id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            return View(nguoiDung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var nguoiDung = await _context.NguoiDung.FindAsync(id);
            _context.NguoiDung.Remove(nguoiDung);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa thành công!";
            return RedirectToAction(nameof(Index));
        }

        private bool NguoiDungExists(int id)
        {
            return _context.NguoiDung.Any(e => e.ID == id);
        }
    }
}