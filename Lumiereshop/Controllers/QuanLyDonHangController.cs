using Microsoft.AspNetCore.Mvc;
using Lumiereshop.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Lumiereshop.Controllers
{
    [TypeFilter(typeof(AdminAuthorizationFilter))] // Yêu cầu quyền admin để truy cập vào các action trong controller này
    public class QuanLyDonHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuanLyDonHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action trang danh sách đơn hàng
        public async Task<IActionResult> Index()
        {
            // Truy vấn danh sách người dùng để hiển thị tên người dùng trong view
            ViewBag.ListNguoiDung = await _context.NguoiDung.ToListAsync();
            // Truy vấn và trả về danh sách đơn hàng từ cơ sở dữ liệu
            return View(await _context.DonHang.OrderByDescending(d => d.ID).ToListAsync());
        }

        // Action hiển thị chi tiết một đơn hàng
        public async Task<IActionResult> Detail(int? id)
        {
            // Kiểm tra xem id của đơn hàng có tồn tại không
            if (id == null)
            {
                return NotFound();
            }

            // Truy vấn thông tin của đơn hàng từ cơ sở dữ liệu
            var donHang = await _context.DonHang.FindAsync(id);
            // Nếu không tìm thấy đơn hàng, trả về trang không tìm thấy
            if (donHang == null)
            {
                return NotFound();
            }

            // Truy vấn danh sách chi tiết đơn hàng tương ứng với id đơn hàng
            ViewBag.ListChiTiet = await _context.ChiTietDonHang.Where(ct => ct.IDDonHang == id).ToListAsync();
            ViewBag.ListNguoiDung = await _context.NguoiDung.ToListAsync();
            // Trả về view chi tiết đơn hàng với thông tin đơn hàng và danh sách chi tiết đơn hàng
            return View(donHang);
        }

        // Action chỉnh sửa đơn hàng
        public async Task<IActionResult> Edit(int? id)
        {
            // Kiểm tra xem id của đơn hàng có tồn tại không
            if (id == null)
            {
                return NotFound();
            }

            // Truy vấn thông tin của đơn hàng từ cơ sở dữ liệu
            var donHang = await _context.DonHang.FindAsync(id);
            // Nếu không tìm thấy đơn hàng, trả về trang không tìm thấy
            if (donHang == null)
            {
                return NotFound();
            }

            // Trả về view chỉnh sửa đơn hàng với thông tin đơn hàng
            return View(donHang);
        }

        // Action xử lý chỉnh sửa đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ten,DiaChi,SoDienThoai,TrangThai")] DonHang donHang)
        {
            // Kiểm tra xem id của đơn hàng có trùng khớp không
            if (id != donHang.ID)
            {
                return NotFound();
            }

            // Kiểm tra dữ liệu nhập vào có hợp lệ không
            if (ModelState.IsValid)
            {
                try
                {
                    // Truy vấn đơn hàng cần chỉnh sửa từ cơ sở dữ liệu
                    var existingDonHang = await _context.DonHang.FindAsync(id);
                    // Nếu không tìm thấy đơn hàng, trả về trang không tìm thấy
                    if (existingDonHang == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật thông tin của đơn hàng đã tồn tại với các giá trị mới
                    existingDonHang.Ten = donHang.Ten;
                    existingDonHang.DiaChi = donHang.DiaChi;
                    existingDonHang.SoDienThoai = donHang.SoDienThoai;
                    existingDonHang.TrangThai = donHang.TrangThai;

                    // Cập nhật đơn hàng trong cơ sở dữ liệu và lưu thay đổi
                    _context.Update(existingDonHang);
                    await _context.SaveChangesAsync();

                    // Hiển thị thông báo thành công và chuyển hướng về trang danh sách đơn hàng
                    TempData["SuccessMessage"] = "Chỉnh sửa đơn hàng thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Nếu có lỗi xảy ra trong quá trình cập nhật, kiểm tra xem đơn hàng có tồn tại không
                    if (!DonHangExists(donHang.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            // Nếu dữ liệu nhập vào không hợp lệ, trả về view chỉnh sửa đơn hàng với dữ liệu đã nhập
            return View(donHang);
        }

        // Action xuất Excel cho đơn hàng theo ID
        public async Task<IActionResult> ExportToExcel(int id)
        {
            // Truy vấn thông tin của đơn hàng từ cơ sở dữ liệu
            var donHang = await _context.DonHang.FindAsync(id);

            // Truy vấn danh sách chi tiết đơn hàng tương ứng với id đơn hàng
            var listChiTiet = await _context.ChiTietDonHang.Where(ct => ct.IDDonHang == id).ToListAsync();

            // Tạo một package Excel
            using (var package = new ExcelPackage())
            {
                // Tạo một sheet trong Excel
                var sheet = package.Workbook.Worksheets.Add("DonHang");
            
                // Tính tổng tiền từ các chi tiết đơn hàng
                decimal tongTien = listChiTiet.Sum(chiTiet => chiTiet.SoLuong * chiTiet.Gia);

                // Ánh xạ trạng thái
                string trangThaiText = string.Empty;
                switch (donHang.TrangThai)
                {
                    case 0:
                        trangThaiText = "Đang xử lý";
                        break;
                    case 1:
                        trangThaiText = "Đã duyệt";
                        break;
                    case 2:
                        trangThaiText = "Đã huỷ";
                        break;
                    case 3:
                        trangThaiText = "Đang giao";
                        break;
                    case 4:
                        trangThaiText = "Đã hoàn thành";
                        break;
                    default:
                        trangThaiText = "Không xác định";
                        break;
                }

                // Thêm thông tin cửa hàng
                sheet.Cells["A1:B1"].Merge = true;
                sheet.Cells["A1"].Value = "Tên cửa hàng";
                sheet.Cells["B1"].Value = "Lumiereshop"; // Thay đổi thành tên cửa hàng thực tế
                sheet.Cells["A1:B1"].Style.Font.Bold = true; // In đậm tiêu đề
                sheet.Cells["A1:B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Căn giữa tiêu đề

                // Thêm dữ liệu hóa đơn vào sheet
                sheet.Cells["A4:B4"].Style.Font.Bold = true; // In đậm tiêu đề
                sheet.Cells["A4"].Value = "ID Hóa đơn";
                sheet.Cells["B4"].Value = donHang.ID;
                sheet.Cells["A5"].Value = "Tên khách hàng";
                sheet.Cells["B5"].Value = donHang.Ten;
                sheet.Cells["A6"].Value = "Địa chỉ";
                sheet.Cells["B6"].Value = donHang.DiaChi;
                sheet.Cells["A7"].Value = "Số điện thoại";
                sheet.Cells["B7"].Value = donHang.SoDienThoai;
                sheet.Cells["A8"].Value = "Tổng tiền";
                sheet.Cells["B8"].Value = tongTien; // Sử dụng tổng tiền đã tính
                sheet.Cells["A9"].Value = "Trạng thái";
                sheet.Cells["B9"].Value = trangThaiText; // Sử dụng trạng thái đã ánh xạ

                // Thêm dữ liệu chi tiết đơn hàng vào sheet
                sheet.Cells["D1:H1"].Merge = true;
                sheet.Cells["D1"].Value = "Chi tiết đơn hàng";
                sheet.Cells["D1:H1"].Style.Font.Bold = true; // In đậm tiêu đề
                sheet.Cells["D1:H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Căn giữa tiêu đề
                sheet.Cells["D3"].Value = "ID";
                sheet.Cells["E3"].Value = "Tên sản phẩm";
                sheet.Cells["F3"].Value = "Số lượng";
                sheet.Cells["G3"].Value = "Giá";
                sheet.Cells["H3"].Value = "Thành tiền";

                int row = 4;
                foreach (var chiTiet in listChiTiet)
                {
                    sheet.Cells[string.Format("D{0}", row)].Value = chiTiet.ID;
                    sheet.Cells[string.Format("E{0}", row)].Value = _context.SanPham.FirstOrDefault(s => s.ID == chiTiet.IDSanPham)?.Ten;
                    sheet.Cells[string.Format("F{0}", row)].Value = chiTiet.SoLuong;
                    sheet.Cells[string.Format("G{0}", row)].Value = chiTiet.Gia + " đ";
                    sheet.Cells[string.Format("H{0}", row)].Value = chiTiet.SoLuong * chiTiet.Gia + " đ";
                    row++;
                }

                // Định dạng các cột
                sheet.Column(1).AutoFit();
                sheet.Column(2).AutoFit();
                sheet.Column(3).AutoFit();
                sheet.Column(4).AutoFit();
                sheet.Column(5).AutoFit();
                sheet.Column(6).AutoFit();
                sheet.Column(7).AutoFit();
                sheet.Column(8).AutoFit();

                // Lưu tệp Excel vào một MemoryStream
                MemoryStream stream = new MemoryStream();
                package.SaveAs(stream);

                // Đặt vị trí của MemoryStream về đầu
                stream.Position = 0;

                // Trả về tệp Excel dưới dạng FileStreamResult
                return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = $"donhang_{donHang.ID}.xlsx"
                };
            }
        }

        // Kiểm tra xem đơn hàng có tồn tại không
        private bool DonHangExists(int id)
        {
            return _context.DonHang.Any(e => e.ID == id);
        }
    }
}