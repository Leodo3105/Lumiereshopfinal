using Lumiereshop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

namespace Lumiereshop.Controllers
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor: Dependency Injection để truy cập cơ sở dữ liệu
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action Index để hiển thị thông tin trên trang quản trị
        public IActionResult Index()
        {
            // Đếm số lượng sản phẩm, người dùng và đơn hàng và gán vào ViewBag
            ViewBag.SoLuongSanPham = _context.SanPham.Count();
            ViewBag.SoLuongNguoiDung = _context.NguoiDung.Count();
            ViewBag.SoLuongDonHang = _context.DonHang.Count();

            // Tính tổng doanh thu của các đơn hàng có trạng thái bằng 4
            ViewBag.TongDoanhThu = (from donHang in _context.DonHang
                                    join chiTiet in _context.ChiTietDonHang on donHang.ID equals chiTiet.IDDonHang
                                    where donHang.TrangThai == 4
                                    select chiTiet.Gia * chiTiet.SoLuong).Sum();

            var doanhThuTheoThang = (from donHang in _context.DonHang
                                     join chiTiet in _context.ChiTietDonHang on donHang.ID equals chiTiet.IDDonHang
                                     where donHang.TrangThai == 4 //Trạng thái 4 mới thống kê
                                     group new { donHang, chiTiet } by new { Thang = donHang.NgayDat.Month, Nam = donHang.NgayDat.Year } into grp
                                     select new
                                     {
                                         Thang = grp.Key.Thang,
                                         Nam = grp.Key.Nam,
                                         TongDoanhThu = grp.Sum(x => x.chiTiet.Gia * x.chiTiet.SoLuong)
                                     })
                                    .OrderByDescending(g => g.Nam)
                                    .ThenByDescending(g => g.Thang)
                                    .Take(12) //Thống kê 12 tháng gần nhất
                                    .OrderBy(g => g.Nam)
                                    .ThenBy(g => g.Thang)
                                    .ToList();

            // Tạo mảng tháng và doanh thu
            var thangArray = doanhThuTheoThang.Select(item => item.Thang).ToArray();
            var doanhThuArray = doanhThuTheoThang.Select(item => item.TongDoanhThu).ToArray();

            var thangNames = doanhThuTheoThang.Select(item => $"{item.Thang}/{item.Nam}").ToArray();

            // Tạo đối tượng chứa các thông tin cần gửi sang view
            var doanhThuTheoThangJson = new
            {
                Thang = thangNames,
                DoanhThu = doanhThuArray
            };

            // Chuyển đổi đối tượng thành chuỗi JSON
            var doanhThuTheoThangJsonString = JsonConvert.SerializeObject(doanhThuTheoThangJson);

            // Gán chuỗi JSON vào ViewBag để truyền sang view
            ViewBag.DoanhThuTheoThang = doanhThuTheoThangJsonString;

            return View();
        }
    }
}
