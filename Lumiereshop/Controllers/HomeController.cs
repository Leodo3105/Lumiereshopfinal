using Lumiereshop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Lumiereshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        // Constructor của HomeController
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // Action method Index trả về trang chính
        public IActionResult Index()
        {
            // Lấy danh sách loại sản phẩm từ cơ sở dữ liệu
            var listLoai = _dbContext.Loai.ToList();
            // Đặt danh sách loại vào ViewBag để truyền sang view
            ViewBag.ListLoai = listLoai;

            // Lấy danh sách sản phẩm từ cơ sở dữ liệu, sắp xếp theo ID giảm dần, chỉ lấy 20 cái
            var listSanPham = _dbContext.SanPham.OrderByDescending(sp => sp.ID).Take(20).ToList();
            // Đặt danh sách sản phẩm vào ViewBag để truyền sang view
            ViewBag.ListSanPham = listSanPham;

            // Lấy danh sách sản phẩm ngẫu nhiên từ cơ sở dữ liệu, chỉ lấy 20 cái
            var ngauNhien = _dbContext.SanPham.OrderBy(r => Guid.NewGuid()).Take(20).ToList();
            // Đặt danh sách sản phẩm ngẫu nhiên vào ViewBag để truyền sang view
            ViewBag.SanPhamNgauNhien = ngauNhien;

            // Lấy danh sách loại sản phẩm từ cơ sở dữ liệu
            var listSlide = _dbContext.Slide.ToList();
            // Đặt danh sách loại vào ViewBag để truyền sang view
            ViewBag.ListSlide = listSlide;

            // Lấy danh sách phản hồi từ cơ sở dữ liệu
            var listPhanHoi = _dbContext.PhanHoi.ToList();
            // Đặt danh sách loại vào ViewBag để truyền sang view
            ViewBag.ListPhanHoi = listPhanHoi;

            // Trả về view Index
            return View();
        }
    }
}
