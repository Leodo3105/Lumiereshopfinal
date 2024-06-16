using Lumiereshop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AdminAuthorizationFilter : IAuthorizationFilter
{
    private readonly ApplicationDbContext _context;

    public AdminAuthorizationFilter(ApplicationDbContext context)
    {
        _context = context;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.Session.GetInt32("UserID");

        // Kiểm tra xem người dùng có đăng nhập không
        if (!user.HasValue)
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }

        // Kiểm tra quyền của người dùng
        var nguoiDung = _context.NguoiDung.FirstOrDefault(u => u.ID == user);

        if (nguoiDung == null || nguoiDung.Quyen != 1)
        {
            // Nếu không có quyền hoặc không tìm thấy người dùng, chuyển hướng đến trang lỗi hoặc trang không có quyền truy cập
            context.Result = new RedirectToActionResult("Login", "Account", null);
        }
    }
}
