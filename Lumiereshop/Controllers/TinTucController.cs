using Lumiereshop.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lumiereshop.Controllers
{
    public class TinTucController : Controller
    {
        private readonly ILogger<HomeController> _logger; // Đối tượng Logger để ghi log
        private readonly ApplicationDbContext _dbContext; // Đối tượng DbContext để tương tác với cơ sở dữ liệu

        // Constructor của TinTucController
        public TinTucController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger; // Khởi tạo đối tượng Logger
            _dbContext = dbContext; // Khởi tạo đối tượng DbContext
        }

        // Action method Index để hiển thị trang danh sách tin tức
        public IActionResult Index()
        {
            // Lấy danh sách tin tức từ cơ sở dữ liệu và đặt vào ViewBag để truyền sang view
            var listTinTuc = _dbContext.TinTuc.ToList();
            ViewBag.ListTinTuc = listTinTuc;

            // Trả về view Index
            return View();
        }

        // Action method ChiTietTinTuc để hiển thị chi tiết một tin tức dựa trên ID
        public IActionResult ChiTietTinTuc(int id)
        {
            // Tìm tin tức trong cơ sở dữ liệu dựa trên ID
            var tinTuc = _dbContext.TinTuc.FirstOrDefault(t => t.ID == id);

            // Kiểm tra nếu không tìm thấy tin tức
            if (tinTuc == null)
            {
                // Trả về trang 404 Not Found
                return NotFound();
            }

            // Trả về view ChiTietTinTuc và truyền vào đối tượng tin tức để hiển thị chi tiết
            return View(tinTuc);
        }
    }
}
