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
    public class QuanLySlideController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuanLySlideController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Slide.OrderByDescending(s => s.ID).ToListAsync());
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slide = await _context.Slide.FirstOrDefaultAsync(m => m.ID == id);
            if (slide == null)
            {
                return NotFound();
            }

            return View(slide);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Anh")] Slide slide, IFormFile anhTep)
        {
            if (ModelState.IsValid)
            {
                if (anhTep != null && anhTep.Length > 0)
                {
                    var fileName = GetUniqueFileName(anhTep.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images/sliders", fileName);

                    // Ensure directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await anhTep.CopyToAsync(stream);
                    }

                    // Gán tên tệp tin cho trường Anh của đối tượng slide
                    slide.Anh = fileName;
                }

                _context.Add(slide);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm mới thành công!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Thông tin không được để trống!";
            return View(slide);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slide = await _context.Slide.FindAsync(id);
            if (slide == null)
            {
                return NotFound();
            }

            return View(slide);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Anh")] Slide slide, IFormFile anhTep)
        {
            if (id != slide.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingSlide = await _context.Slide.FindAsync(id);

                    if (existingSlide == null)
                    {
                        return NotFound();
                    }

                    if (anhTep != null && anhTep.Length > 0)
                    {
                        // Xoá file ảnh cũ nếu tồn tại
                        if (!string.IsNullOrEmpty(existingSlide.Anh))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images/sliders", existingSlide.Anh);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Lưu trữ tên tệp tin mới trong trường Anh của slide
                        var fileName = GetUniqueFileName(anhTep.FileName);
                        existingSlide.Anh = fileName;

                        // Lưu trữ tập tin ảnh mới trong thư mục wwwroot/client/images
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images/sliders", fileName);
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await anhTep.CopyToAsync(stream);
                        }
                    }

                    _context.Update(existingSlide);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlideExists(slide.ID))
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
            return View(slide);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slide = await _context.Slide.FirstOrDefaultAsync(m => m.ID == id);
            if (slide == null)
            {
                return NotFound();
            }

            return View(slide);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var slide = await _context.Slide.FindAsync(id);
            if (slide == null)
            {
                return NotFound();
            }

            try
            {
                // Xoá file ảnh nếu tồn tại
                if (!string.IsNullOrEmpty(slide.Anh))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images/sliders", slide.Anh);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Slide.Remove(slide);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi trong quá trình xóa slide!";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool SlideExists(int id)
        {
            return _context.Slide.Any(e => e.ID == id);
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid().ToString().Substring(0, 8) + Path.GetExtension(fileName);
        }
    }
}
