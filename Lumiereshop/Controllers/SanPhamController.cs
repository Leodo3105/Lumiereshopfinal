using Lumiereshop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lumiereshop.Controllers
{
	public class SanPhamController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext _dbContext;
		public SanPhamController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
		{
			_logger = logger;
			_dbContext = dbContext;
		}
		public IActionResult ChiTietSanPham(int id)
		{
			var sanPham = _dbContext.SanPham.FirstOrDefault(t => t.ID == id);
			if (sanPham == null)
			{
				return NotFound();
			}

			return View(sanPham);
		}
        public IActionResult DanhMuc(int id)
        {
            // Lấy danh sách loại sản phẩm từ cơ sở dữ liệu
            var listLoai = _dbContext.Loai.ToList();
            // Đặt danh sách loại vào ViewBag để truyền sang view
            ViewBag.ListLoai = listLoai;

            // Lấy danh sách sản phẩm dựa trên idLoai được truyền vào
            var listSanPham = _dbContext.SanPham.Where(sp => sp.IDLoai == id).ToList();

            return View(listSanPham); // Trả về view với danh sách sản phẩm
        }
        // Action method để tìm kiếm sản phẩm
        public IActionResult TimKiem(string keyword)
        {
            // Lấy danh sách loại sản phẩm từ cơ sở dữ liệu
            var listLoai = _dbContext.Loai.ToList();
            // Đặt danh sách loại vào ViewBag để truyền sang view
            ViewBag.ListLoai = listLoai;

            // Kiểm tra xem từ khóa tìm kiếm có tồn tại không
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return NotFound();
            }

            // Sử dụng LINQ để tìm kiếm sản phẩm theo từ khóa
            var listSanPham = _dbContext.SanPham.Where(sp => sp.Ten.Contains(keyword)).ToList();

            // Trả về view hiển thị danh sách sản phẩm tìm được
            return View("DanhMuc", listSanPham);
        }
    }
}
