using Microsoft.AspNetCore.Mvc;
using Lumiereshop.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lumiereshop.Controllers
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class QuanLySanPhamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuanLySanPhamController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ListLoai = await _context.Loai.ToListAsync();
            return View(await _context.SanPham.OrderByDescending(s => s.ID).ToListAsync());
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPham
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewBag.ListLoai = await _context.Loai.ToListAsync();
            return View(sanPham);
        }

        public IActionResult Create()
        {
            ViewBag.ListLoai = _context.Loai.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Ten,Gia,SoLuong,Size,LuuY,IDLoai")] SanPham sanPham, IFormFile anhTep)
        {
            if (ModelState.IsValid)
            {
                if (anhTep != null && anhTep.Length > 0)
                {
                    var fileName = GetUniqueFileName(anhTep.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images/products", fileName);

                    // Ensure directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await anhTep.CopyToAsync(stream);
                    }

                    // Gán tên tệp tin cho trường Anh của đối tượng sanPham
                    sanPham.Anh = fileName;
                }

                _context.SanPham.Add(sanPham);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm mới thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ, gán danh sách loại vào ViewBag và hiển thị lại view
            ViewBag.ListLoai = _context.Loai.ToList();
            TempData["ErrorMessage"] = "Thông tin không được để trống!";
            return View(sanPham);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewBag.ListLoai = _context.Loai.ToList();
            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ten,Gia,SoLuong,Size,LuuY,IDLoai")] SanPham sanPham, IFormFile anhTep)
        {
            if (id != sanPham.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingSanPham = await _context.SanPham.FindAsync(id);

                    if (existingSanPham == null)
                    {
                        return NotFound();
                    }

                    existingSanPham.Ten = sanPham.Ten;
                    existingSanPham.Gia = sanPham.Gia;
                    existingSanPham.SoLuong = sanPham.SoLuong;
                    existingSanPham.Size = sanPham.Size;
                    existingSanPham.LuuY = sanPham.LuuY;
                    existingSanPham.IDLoai = sanPham.IDLoai;

                    if (anhTep != null && anhTep.Length > 0)
                    {
                        // Xoá file ảnh cũ nếu tồn tại
                        if (!string.IsNullOrEmpty(existingSanPham.Anh))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images/products", existingSanPham.Anh);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Lấy tên file duy nhất
                        var uniqueFileName = GetUniqueFileName(anhTep.FileName);

                        // Lưu trữ tên tệp tin mới trong trường Anh của sản phẩm
                        existingSanPham.Anh = uniqueFileName;

                        // Lưu trữ tập tin ảnh mới trong thư mục wwwroot/client/images
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images/products", uniqueFileName);
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await anhTep.CopyToAsync(stream);
                        }
                    }

                    _context.Update(existingSanPham);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.ID))
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
            ViewBag.ListLoai = _context.Loai.ToList();
            return View(sanPham);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPham
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }

            try
            {
                // Xoá file ảnh nếu tồn tại
                if (!string.IsNullOrEmpty(sanPham.Anh))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/images/products", sanPham.Anh);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.SanPham.Remove(sanPham);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi trong quá trình xóa sản phẩm!";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool SanPhamExists(int id)
        {
            return _context.SanPham.Any(e => e.ID == id);
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid().ToString().Substring(0, 8) + Path.GetExtension(fileName);
        }
    }
}