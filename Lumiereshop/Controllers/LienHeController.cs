using Lumiereshop.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lumiereshop.Controllers
{
    public class LienHeController : Controller
    {
        private readonly ILogger<HomeController> _logger; // Đối tượng Logger để ghi log
        private readonly ApplicationDbContext _dbContext; // Đối tượng DbContext để tương tác với cơ sở dữ liệu

        // Constructor của TinTucController
        public LienHeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger; // Khởi tạo đối tượng Logger
            _dbContext = dbContext; // Khởi tạo đối tượng DbContext
        }

        // Action method Index để hiển thị trang danh sách liên hệ
        public IActionResult Index()
        {
            // Lấy danh sách liên hệ từ cơ sở dữ liệu và đặt vào ViewBag để truyền sang view
            var listLienHe = _dbContext.LienHe.ToList();
            ViewBag.ListLienHe = listLienHe;

            // Trả về view Index
            return View();
        }
    }
}
