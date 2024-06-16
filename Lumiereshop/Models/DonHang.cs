namespace Lumiereshop.Models
{
    public class DonHang
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public DateTime NgayDat { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public int TrangThai { get; set; }
        public int? IDNguoiDung { get; set; }
    }
}
