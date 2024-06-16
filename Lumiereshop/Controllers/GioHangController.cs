using BanVe.Pages; // Import các namespaces cần thiết
using Lumiereshop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Lumiereshop.Controllers
{
    public class GioHangController : Controller // Định nghĩa class GioHangController kế thừa từ Controller
    {
        private readonly ILogger<HomeController> _logger; // Đối tượng Logger để ghi log
        private readonly ApplicationDbContext _dbContext; // Đối tượng DbContext để tương tác với cơ sở dữ liệu
        private IConfiguration _configuration; // Đối tượng IConfiguration để đọc cấu hình
        public GioHangController(ILogger<HomeController> logger, ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _logger = logger; // Khởi tạo đối tượng Logger
            _dbContext = dbContext; // Khởi tạo đối tượng DbContext
            _configuration = configuration; // Khởi tạo đối tượng IConfiguration
        }

        public List<Cart> CartItems { get; set; } // Danh sách các sản phẩm trong giỏ hàng

        // Action method Index để hiển thị trang giỏ hàng
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId != null) // Nếu có userID trong Session
            {
                var user = _dbContext.NguoiDung.FindAsync(userId).Result;
                if (user != null)
                {
                    ViewBag.User = user; // Đặt thông tin người dùng vào ViewBag để truyền sang view
                }
                else
                {
                    ViewBag.User = new NguoiDung(); // Nếu không tìm thấy người dùng, tạo một đối tượng người dùng mới
                }
            }

            LoadCart(); // Load giỏ hàng từ Session
            return View(); // Trả về view Index
        }

        // Phương thức LoadCart để load giỏ hàng từ Session
        private void LoadCart()
        {
            var sessionCart = HttpContext.Session.GetString("Cart");

            if (sessionCart != null) // Nếu có giỏ hàng trong Session
            {
                CartItems = JsonConvert.DeserializeObject<List<Cart>>(sessionCart); // Deserialize giỏ hàng từ JSON
                ViewBag.CartItems = CartItems; // Đặt giỏ hàng vào ViewBag để truyền sang view

                var TongTien = CartItems.Sum(item => item.Gia * item.SoLuong); // Tính tổng số tiền
                var SoLuong = CartItems.Sum(item => item.SoLuong); // Tính tổng số lượng sản phẩm
                ViewBag.TongTien = TongTien; // Đặt tổng số tiền vào ViewBag
                ViewBag.SoLuong = SoLuong; // Đặt tổng số lượng sản phẩm vào ViewBag
            }
            else // Nếu không có giỏ hàng trong Session
            {
                ViewBag.CartItems = new List<Cart>(); // Đặt giỏ hàng rỗng vào ViewBag
                ViewBag.TongTien = 0; // Đặt tổng số tiền bằng 0 vào ViewBag
                ViewBag.SoLuong = 0; // Đặt tổng số lượng sản phẩm bằng 0 vào ViewBag
            }
        }

        // Action method AddToCart để thêm sản phẩm vào giỏ hàng
        public IActionResult AddToCart(int id)
        {
            var sanPham = _dbContext.SanPham.Find(id);

            var sessionCart = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(sessionCart) ? new List<Cart>() : JsonConvert.DeserializeObject<List<Cart>>(sessionCart);

            var existingCartItem = cart.FirstOrDefault(item => item.ID == id);
            if (existingCartItem != null)
            {
                existingCartItem.SoLuong++; // Tăng số lượng sản phẩm nếu đã tồn tại trong giỏ hàng
            }
            else
            {
                var cartItem = new Cart // Tạo mới một đối tượng Cart nếu sản phẩm chưa tồn tại trong giỏ hàng
                {
                    ID = sanPham.ID,
                    Anh = sanPham.Anh,
                    Ten = sanPham.Ten,
                    SoLuong = 1,
                    Gia = sanPham.Gia
                };
                cart.Add(cartItem); // Thêm sản phẩm vào giỏ hàng
            }

            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart)); // Lưu giỏ hàng vào Session

			// Thêm thông báo vào TempData
			TempData["SuccessMessage"] = "Sản phẩm đã được thêm vào giỏ hàng thành công.";

			string returnUrl = Request.Headers["Referer"].ToString(); // Lấy URL trang trước đó
            if (!string.IsNullOrEmpty(returnUrl)) // Nếu URL trang trước đó không rỗng
            {
                return Redirect(returnUrl); // Chuyển hướng về trang trước đó
            }
            else
            {
                // Nếu không có trang trước đó, chuyển hướng về trang chủ
                return RedirectToAction("Index", "Home");
            }
        }

        // Action method PlusCart để tăng số lượng sản phẩm trong giỏ hàng
        public IActionResult PlusCart(int id)
        {
            var sessionCart = HttpContext.Session.GetString("Cart");

            var cart = string.IsNullOrEmpty(sessionCart) ? new List<Cart>() : JsonConvert.DeserializeObject<List<Cart>>(sessionCart);

            var existingCartItem = cart.FirstOrDefault(item => item.ID == id);

            existingCartItem.SoLuong++; // Tăng số lượng sản phẩm

            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart)); // Lưu giỏ hàng vào Session

            LoadCart(); // Load lại giỏ hàng

            return RedirectToAction("Index", "GioHang"); // Chuyển hướng về trang giỏ hàng
        }

        // Action method MinusCart để giảm số lượng sản phẩm trong giỏ hàng
        public IActionResult MinusCart(int id)
        {
            var sessionCart = HttpContext.Session.GetString("Cart");

            var cart = string.IsNullOrEmpty(sessionCart) ? new List<Cart>() : JsonConvert.DeserializeObject<List<Cart>>(sessionCart);

            var existingCartItem = cart.FirstOrDefault(item => item.ID == id);

            if (existingCartItem != null)
            {
                existingCartItem.SoLuong--; // Giảm số lượng sản phẩm

                if (existingCartItem.SoLuong <= 0) // Nếu số lượng sản phẩm nhỏ hơn hoặc bằng 0
                {
                    cart.Remove(existingCartItem); // Xóa sản phẩm khỏi giỏ hàng
                }

                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart)); // Lưu giỏ hàng vào Session
            }

            LoadCart(); // Load lại giỏ hàng

            return RedirectToAction("Index", "GioHang"); // Chuyển hướng về trang giỏ hàng
        }

        // Action method DeleteCart để xóa sản phẩm khỏi giỏ hàng
        public IActionResult DeleteCart(int id)
        {
            var sessionCart = HttpContext.Session.GetString("Cart");

            var cart = string.IsNullOrEmpty(sessionCart) ? new List<Cart>() : JsonConvert.DeserializeObject<List<Cart>>(sessionCart);

            var existingCartItem = cart.FirstOrDefault(item => item.ID == id);

            if (existingCartItem != null)
            {
                cart.Remove(existingCartItem); // Xóa sản phẩm khỏi giỏ hàng

                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart)); // Lưu giỏ hàng vào Session
            }

            LoadCart(); // Load lại giỏ hàng

            return RedirectToAction("Index", "GioHang"); // Chuyển hướng về trang giỏ hàng
        }

        // Action method ThanhToan để thực hiện thanh toán giỏ hàng
        [HttpPost]
        public IActionResult ThanhToan(string ten, string diachi, string sodienthoai)
        {
            var sessionCart = HttpContext.Session.GetString("Cart");

            if (string.IsNullOrEmpty(sessionCart)) // Kiểm tra nếu giỏ hàng rỗng
            {
                TempData["ErrorMessage"] = "Chưa có sản phẩm trong giỏ hàng";

                // Nếu giỏ hàng rỗng, không thực hiện thanh toán và trả về trang Index hoặc thông báo lỗi
                return RedirectToAction("Index", "GioHang"); // Hoặc trả về trang Index của giỏ hàng
            }

            if (string.IsNullOrEmpty(ten) || string.IsNullOrEmpty(diachi) || string.IsNullOrEmpty(sodienthoai))  // Kiểm tra nếu thông tin rỗng
            {
                TempData["ErrorMessage"] = "Nhập đầy đủ thông tin giao hàng";

                return RedirectToAction("Index", "GioHang"); // Chuyển hướng người dùng đến trang Index của giỏ hàng
            }

            CartItems = JsonConvert.DeserializeObject<List<Cart>>(sessionCart);

            DonHang donHang = new DonHang
            {
                Ten = ten,
                NgayDat = DateTime.Now,
                DiaChi = diachi,
                SoDienThoai = sodienthoai,
                TrangThai = 0,
            };

            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId.HasValue) // Kiểm tra xem userId có giá trị hay không
            {
                donHang.IDNguoiDung = userId.Value; // Sử dụng giá trị của userId nếu có
            }

            _dbContext.DonHang.Add(donHang);
            _dbContext.SaveChanges();

            long totalAmount = 0;

            foreach (var chiTiet in CartItems)
            {
                ChiTietDonHang ct = new ChiTietDonHang
                {
                    IDSanPham = chiTiet.ID,
                    SoLuong = chiTiet.SoLuong,
                    Gia = chiTiet.Gia,
                    IDDonHang = donHang.ID,
                };
                _dbContext.ChiTietDonHang.Add(ct);

                totalAmount += chiTiet.Gia * chiTiet.SoLuong;
            }

            _dbContext.SaveChanges();

            HttpContext.Session.Remove("Cart"); // Xóa giỏ hàng khỏi Session sau khi thanh toán

            var model = new PaymentInformationModel
            {
                Amount = totalAmount,
                OrderId = donHang.ID.ToString(),
                OrderDescription = "",
                OrderType = "other",
                Url = "/GioHang/CallBack"
            };

            var paymentUrl = CreatePaymentUrl(model, HttpContext); // Tạo URL thanh toán

            return Redirect(paymentUrl); // Chuyển hướng đến URL thanh toán
        }

        // Phương thức CreatePaymentUrl để tạo URL thanh toán
        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

            var pay = new VnPayLibrary(); // Tạo mới một đối tượng VnPayLibrary

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]); // Thêm dữ liệu yêu cầu cho thanh toán
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context)); // Lấy địa chỉ IP của client
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan cho don hang: " + model.OrderId);
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", "https://localhost:7107/GioHang/CallBack"); // URL callback sau khi thanh toán
            pay.AddRequestData("vnp_TxnRef", model.OrderId.ToString());

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]); // Tạo URL thanh toán

            return paymentUrl; // Trả về URL thanh toán
        }
		// Action method CallBack để xử lý kết quả thanh toán từ cổng thanh toán
		public IActionResult CallBack()
		{
			// Lấy các tham số từ query string
			var vnp_ResponseCode = Request.Query["vnp_ResponseCode"];

			// Kiểm tra mã phản hồi từ cổng thanh toán
			bool success = vnp_ResponseCode == "00";

			if (success)
			{
				TempData["SuccessMessage"] = "Cảm ơn bạn đã thanh toán thành công!";
			}
			else
			{
				TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình thanh toán. Vui lòng thử lại sau.";
			}

			return View(); // Trả về view CallBack
		}
	}
}