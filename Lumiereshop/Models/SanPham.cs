namespace Lumiereshop.Models
{
    public class SanPham
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public string? Anh { get; set; }
        public int Gia { get; set; }
        public int SoLuong { get; set; }
        public string Size { get; set; }
        public string? LuuY { get; set; }
        public int IDLoai { get; set; }
    }
}
