using Microsoft.AspNetCore.Mvc;
using Lumiereshop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lumiereshop.Controllers
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class QuanLyThamSoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuanLyThamSoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ThamSo.ToListAsync());
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thamSo = await _context.ThamSo.FirstOrDefaultAsync(m => m.ID == id);
            if (thamSo == null)
            {
                return NotFound();
            }

            return View(thamSo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thamSo = await _context.ThamSo.FindAsync(id);
            if (thamSo == null)
            {
                return NotFound();
            }

            return View(thamSo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,NoiDung")] ThamSo thamSo, IFormFile anhTep)
        {
            if (id != thamSo.ID)
            {
                return NotFound();
            }

            try
            {
                var existingThamSo = await _context.ThamSo.FindAsync(id);

                if (existingThamSo == null)
                {
                    return NotFound();
                }

                existingThamSo.NoiDung = thamSo.NoiDung;

                if (anhTep != null && anhTep.Length > 0)
                {
                    // Xoá file ảnh cũ nếu tồn tại
                    if (!string.IsNullOrEmpty(existingThamSo.Anh))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images", existingThamSo.Anh);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Lấy tên file duy nhất
                    var uniqueFileName = GetUniqueFileName(anhTep.FileName);

                    // Lưu trữ tên tệp tin mới trong trường Anh của sản phẩm
                    existingThamSo.Anh = uniqueFileName;

                    // Lưu trữ tập tin ảnh mới trong thư mục wwwroot/client/images
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images", uniqueFileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await anhTep.CopyToAsync(stream);
                    }
                }

                _context.Update(existingThamSo);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThamSoExists(thamSo.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            TempData["ErrorMessage"] = "Thông tin không được để trống!";
            return View(thamSo);
        }

        private bool ThamSoExists(int id)
        {
            return _context.ThamSo.Any(e => e.ID == id);
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid().ToString().Substring(0, 8) + Path.GetExtension(fileName);
        }
    }
}
