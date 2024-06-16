using Microsoft.EntityFrameworkCore;

namespace Lumiereshop.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<TinTuc> TinTuc { get; set; }
        public DbSet<Loai> Loai { get; set; }
        public DbSet<SanPham> SanPham { get; set; }
        public DbSet<DonHang> DonHang { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }
        public DbSet<LienHe> LienHe { get; set; }
        public DbSet<Slide> Slide { get; set; }
        public DbSet<ThamSo> ThamSo { get; set; }
        public DbSet<PhanHoi> PhanHoi { get; set; }
    }
}
