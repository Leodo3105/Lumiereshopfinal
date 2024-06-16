using Lumiereshop.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;

namespace Lumiereshop.Controllers
{
    public class AccountController : Controller // Định nghĩa class AccountController kế thừa từ Controller
    {
        private readonly ILogger<HomeController> _logger; // Đối tượng Logger để ghi log
        private readonly ApplicationDbContext _dbContext; // Đối tượng DbContext để tương tác với cơ sở dữ liệu
        public AccountController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger; // Khởi tạo đối tượng Logger
            _dbContext = dbContext; // Khởi tạo đối tượng DbContext
        }

        // Action method Login để hiển thị trang đăng nhập
        public IActionResult Login()
        {
            return View(); // Trả về view Login
        }

        // Action method Login để xử lý quá trình đăng nhập
        [HttpPost]
        public IActionResult Login(string taikhoan, string matkhau)
        {
            // Hash mật khẩu người dùng nhập vào
            string hashedPassword = HashPassword(matkhau);

            try
            {
                var user = _dbContext.NguoiDung.FirstOrDefault(u => u.TaiKhoan == taikhoan && u.MatKhau == hashedPassword); // Tìm người dùng trong cơ sở dữ liệu

                if (user == null) // Nếu không tìm thấy người dùng
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác."); // Thêm lỗi vào ModelState
                    return View(); // Trả về view Login
                }

                HttpContext.Session.SetInt32("UserID", (int)user.ID); // Lưu ID người dùng vào Session

                if (user.Quyen == 1) // Nếu là admin
                {
                    return RedirectToAction("Index", "Admin"); // Chuyển hướng đến trang quản trị
                }
                else if (user.Quyen == 2) // Nếu là người dùng thông thường
                {
                    return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chính
                }
                else // Nếu không phải là admin hoặc người dùng thông thường
                {
                    return RedirectToPage("/Error"); // Chuyển hướng đến trang lỗi
                }
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (tùy chọn)
                // _logger.LogError(ex, "Đã xảy ra lỗi trong quá trình đăng nhập.");

                ModelState.AddModelError("", "Đã có lỗi xảy ra trong quá trình đăng nhập. Vui lòng thử lại.");
                return View();
            }
        }
        // Action method Signup để hiển thị trang đăng ký
        public IActionResult Signup()
        {
            return View(); // Trả về view Signup
        }

        // Action method Signup để xử lý quá trình đăng ký
        [HttpPost]
        public IActionResult Signup(string taikhoan, string ten, string diachi, string matkhau, string nhaplaimatkhau, string sodienthoai)
        {
            if (matkhau != nhaplaimatkhau) // Kiểm tra mật khẩu nhập lại có trùng khớp không
            {
                ModelState.AddModelError("MatKhau", "Mật khẩu nhập lại không trùng khớp."); // Thêm lỗi vào ModelState
                return View(); // Trả về view Signup
            }
            // Kiểm tra các trường bắt buộc có được điền đầy đủ không
            if (string.IsNullOrWhiteSpace(taikhoan) || string.IsNullOrWhiteSpace(ten) ||
                string.IsNullOrWhiteSpace(diachi) || string.IsNullOrWhiteSpace(matkhau))
            {
                ModelState.AddModelError("", "Vui lòng điền đầy đủ thông tin.");
                return View();
            }
            var nguoiDung = new NguoiDung // Tạo mới đối tượng người dùng
            {
                TaiKhoan = taikhoan,
                Ten = ten,
                DiaChi = diachi,
                //MatKhau = matkhau,
                SoDienThoai = sodienthoai,
                MatKhau = HashPassword(matkhau),
                Quyen = 2 // Quyền mặc định của người dùng mới là 2 (người dùng thông thường)
            };

            if (ModelState.IsValid) // Kiểm tra ModelState có hợp lệ không
            {
                if (_dbContext.NguoiDung.Any(u => u.TaiKhoan == taikhoan)) // Kiểm tra tên đăng nhập đã tồn tại hay chưa
                {
                    ModelState.AddModelError("TaiKhoan", "Tên đăng nhập đã tồn tại."); // Thêm lỗi vào ModelState
                    return View(nguoiDung); // Trả về view Signup với dữ liệu của người dùng mới
                }

                _dbContext.NguoiDung.Add(nguoiDung); // Thêm người dùng vào cơ sở dữ liệu
                _dbContext.SaveChanges(); // Lưu thay đổi

                return RedirectToAction("Login"); // Chuyển hướng đến trang đăng nhập
            }

            return View(nguoiDung); // Trả về view Signup với dữ liệu của người dùng mới
        }
        // Phương thức hỗ trợ để hash mật khẩu
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        // Action method Login để hiển thị trang tài khoản
        public IActionResult TaiKhoan()
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId != null) // Nếu có userID trong Session
            {
                var user = _dbContext.NguoiDung.FindAsync(userId).Result;
                ViewBag.User = user;

                // Lấy danh sách đơn hàng của người dùng
                ViewBag.ListDonHang = _dbContext.DonHang.Where(dh => dh.IDNguoiDung == userId).ToList();
            }

            return View(); // Trả về view TaiKhoan
        }


        // Action method để xử lý việc cập nhật thông tin người dùng
        [HttpPost]
        public IActionResult TaiKhoan(string matkhau, string nhaplaimatkhau, string ten, string diachi)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            ViewBag.ListDonHang = _dbContext.DonHang.Where(dh => dh.IDNguoiDung == userId).ToList();

            if (userId != null) // Nếu có userID trong Session
            {
                var user = _dbContext.NguoiDung.FindAsync(userId).Result;

                if (user != null) // Nếu tìm thấy người dùng trong cơ sở dữ liệu
                {
                    // Cập nhật thông tin người dùng
                    user.Ten = ten;
                    user.DiaChi = diachi;

                    // Kiểm tra và cập nhật mật khẩu nếu được chỉnh sửa
                    if (!string.IsNullOrEmpty(matkhau) && !string.IsNullOrEmpty(nhaplaimatkhau))
                    {
                        if (matkhau != nhaplaimatkhau)
                        {
                            TempData["ErrorMessage"] = "Mật khẩu nhập lại không trùng khớp.";
                            return View(); // Trả về view với thông báo lỗi nếu mật khẩu nhập lại không trùng khớp
                        }
                        user.MatKhau = matkhau;
                    }

                    _dbContext.SaveChanges(); // Lưu thay đổi

                    TempData["SuccessMessage"] = "Thông tin của bạn đã được cập nhật thành công.";
                    ViewBag.User = user;
                    return View(); // Trả về view TaiKhoan với thông báo cập nhật thành công
                }
            }
            TempData["ErrorMessage"] = "Đã xảy ra lỗi.";
            return RedirectToAction("TaiKhoan", "Account"); // Nếu không có userID trong Session hoặc không tìm thấy người dùng trong cơ sở dữ liệu, chuyển hướng đến trang đăng nhập
        }

        // Action method Logout để đăng xuất người dùng
        public IActionResult ResetPass()
        {
            ViewBag.check = false;
            ViewBag.otp = false;
            return View();
        }
        [HttpPost]
        public IActionResult ResetPass(string taikhoan, string? password, string? password2, string? otp)
        {
            var user = _dbContext.NguoiDung.FirstOrDefault(x => x.TaiKhoan == taikhoan || x.SoDienThoai == taikhoan); // tìm trong cơ sở dữ liệu coi có ng dùng này không
            // nêu không
            if (user == null)
            {
                TempData["ErrorMessage"] = "Tài Khoản Không Tồn Tại";
                return View();
            }
            if (user != null && password == null && password2 == null && otp == null)
            {
                ViewBag.value = taikhoan;
                ViewBag.otp = true;
                GenerateAndSendOtp(taikhoan);
                return View();
            }
            if (user != null && password == null && password2 == null && otp != null)
            {
                var otpcheck =  Request.Cookies["otp"];
                if (otpcheck == otp)
                {
                    ViewBag.value = taikhoan;
                    ViewBag.otp = false;
                    ViewBag.check = true;
                    Response.Cookies.Delete("otp");
                    return View();
                }
                else
                {
                    ViewBag.value = taikhoan;
                    ViewBag.otp = true;
                  
                    return View();
                }
            }
            if (user != null && password != null && password2 != null && otp == null)
            {
                var mk = HashPassword(password);
                user.MatKhau = mk;
                _dbContext.NguoiDung.Update(user);
                _dbContext.SaveChanges();
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        private string GenerateAndSendOtp(string email)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            SetOtpCookie(otp);
            SendEmailAsync(email, otp);
            return otp;
        }

        private void SetOtpCookie(string otp)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(10),
                HttpOnly = true
            };

           Response.Cookies.Append("otp", otp);
        }

        public async Task SendEmailAsync(string email, string otp)
        {
            const string subject = "Your OTP Code";
            string body = $"Your OTP code is {otp}";
            var host = "smtp.gmail.com";
            var port = 587;
            var username = "duchuyn091@gmail.com";
            var password = "lnnpwignooozcqpn";
            var senderName = "Reset password";
            var senderEmail = "hsurtr376@gmail.com";
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(senderName, senderEmail));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };
            using (var client = new SmtpClient())
            {
                client.Connect(host, port, false);
                client.Authenticate(username, password);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Xóa Session
            return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập
        }
    }
}