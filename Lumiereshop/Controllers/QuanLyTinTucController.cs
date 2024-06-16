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
    public class QuanLyTinTucController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuanLyTinTucController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.TinTuc.OrderByDescending(t => t.ID).ToListAsync());
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.TinTuc.FirstOrDefaultAsync(m => m.ID == id);
            if (tinTuc == null)
            {
                return NotFound();
            }

            return View(tinTuc);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Ten,NoiDung")] TinTuc tinTuc, IFormFile anhTep)
        {
            if (ModelState.IsValid)
            {
                if (anhTep != null && anhTep.Length > 0)
                {
                    var fileName = GetUniqueFileName(anhTep.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images/blog", fileName);

                    // Ensure directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await anhTep.CopyToAsync(stream);
                    }

                    // Gán tên tệp tin cho trường Anh của đối tượng tinTuc
                    tinTuc.Anh = fileName;
                }

                tinTuc.NgayDang = DateTime.Now;

                _context.Add(tinTuc);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm mới thành công!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Thông tin không được để trống!";
            return View(tinTuc);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.TinTuc.FindAsync(id);
            if (tinTuc == null)
            {
                return NotFound();
            }

            return View(tinTuc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ten,NoiDung")] TinTuc tinTuc, IFormFile anhTep)
        {
            if (id != tinTuc.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingTinTuc = await _context.TinTuc.FindAsync(id);

                    if (existingTinTuc == null)
                    {
                        return NotFound();
                    }

                    existingTinTuc.Ten = tinTuc.Ten;
                    existingTinTuc.NoiDung = tinTuc.NoiDung;

                    if (anhTep != null && anhTep.Length > 0)
                    {
                        // Xoá file ảnh cũ nếu tồn tại
                        if (!string.IsNullOrEmpty(existingTinTuc.Anh))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images/blog", existingTinTuc.Anh);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Lưu trữ tên tệp tin mới trong trường Anh của tinTuc
                        var fileName = GetUniqueFileName(anhTep.FileName);
                        existingTinTuc.Anh = fileName;

                        // Lưu trữ tập tin ảnh mới trong thư mục wwwroot/client/images
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images/blog", fileName);
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await anhTep.CopyToAsync(stream);
                        }
                    }

                    _context.Update(existingTinTuc);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TinTucExists(tinTuc.ID))
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
            return View(tinTuc);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.TinTuc.FirstOrDefaultAsync(m => m.ID == id);
            if (tinTuc == null)
            {
                return NotFound();
            }

            return View(tinTuc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var tinTuc = await _context.TinTuc.FindAsync(id);
            if (tinTuc == null)
            {
                return NotFound();
            }

            try
            {
                // Xoá file ảnh nếu tồn tại
                if (!string.IsNullOrEmpty(tinTuc.Anh))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images/news", tinTuc.Anh);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.TinTuc.Remove(tinTuc);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi trong quá trình xóa tin tức!";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool TinTucExists(int id)
        {
            return _context.TinTuc.Any(e => e.ID == id);
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid().ToString().Substring(0, 8) + Path.GetExtension(fileName);
        }
    }
}
