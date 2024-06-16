using Microsoft.AspNetCore.Mvc;
using Lumiereshop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lumiereshop.Controllers
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class QuanLyPhanHoiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuanLyPhanHoiController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.PhanHoi.OrderByDescending(p => p.ID).ToListAsync());
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phanHoi = await _context.PhanHoi.FirstOrDefaultAsync(m => m.ID == id);
            if (phanHoi == null)
            {
                return NotFound();
            }

            return View(phanHoi);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Ten,ChucDanh,NoiDung")] PhanHoi phanHoi, IFormFile anhTep)
        {
            if (ModelState.IsValid)
            {
                if (anhTep != null && anhTep.Length > 0)
                {
                    var fileName = GetUniqueFileName(anhTep.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images/testimonials", fileName);

                    // Ensure directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await anhTep.CopyToAsync(stream);
                    }

                    // Assign the file name to the Anh property of the phanHoi object
                    phanHoi.Anh = fileName;
                }

                _context.Add(phanHoi);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm mới thành công!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Thông tin không được để trống!";
            return View(phanHoi);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phanHoi = await _context.PhanHoi.FindAsync(id);
            if (phanHoi == null)
            {
                return NotFound();
            }

            return View(phanHoi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ten,ChucDanh,NoiDung")] PhanHoi phanHoi, IFormFile anhTep)
        {
            if (id != phanHoi.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPhanHoi = await _context.PhanHoi.FindAsync(id);

                    if (existingPhanHoi == null)
                    {
                        return NotFound();
                    }

                    existingPhanHoi.Ten = phanHoi.Ten;
                    existingPhanHoi.ChucDanh = phanHoi.ChucDanh;
                    existingPhanHoi.NoiDung = phanHoi.NoiDung;

                    if (anhTep != null && anhTep.Length > 0)
                    {
                        // Xoá file ảnh cũ nếu tồn tại
                        if (!string.IsNullOrEmpty(existingPhanHoi.Anh))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images/testimonials", existingPhanHoi.Anh);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Lấy tên file duy nhất
                        var uniqueFileName = GetUniqueFileName(anhTep.FileName);

                        // Lưu trữ tên tệp tin mới trong trường Anh của sản phẩm
                        existingPhanHoi.Anh = uniqueFileName;

                        // Lưu trữ tập tin ảnh mới trong thư mục wwwroot/client/images
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images/testimonials", uniqueFileName);
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await anhTep.CopyToAsync(stream);
                        }
                    }

                    _context.Update(existingPhanHoi);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhanHoiExists(phanHoi.ID))
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
            return View(phanHoi);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phanHoi = await _context.PhanHoi.FirstOrDefaultAsync(m => m.ID == id);
            if (phanHoi == null)
            {
                return NotFound();
            }

            return View(phanHoi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var phanHoi = await _context.PhanHoi.FindAsync(id);
            if (phanHoi == null)
            {
                return NotFound();
            }

            try
            {
                // Xoá file ảnh nếu tồn tại
                if (!string.IsNullOrEmpty(phanHoi.Anh))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images/feedbacks", phanHoi.Anh);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.PhanHoi.Remove(phanHoi);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi trong quá trình xóa phản hồi!";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool PhanHoiExists(int id)
        {
            return _context.PhanHoi.Any(e => e.ID == id);
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid().ToString().Substring(0, 8) + Path.GetExtension(fileName);
        }
    }
}
