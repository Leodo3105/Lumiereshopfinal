namespace Lumiereshop.Models
{
    public class Cart
    {
        public int ID { get; set; }
        public string Anh { get; set; }
        public string Ten { get; set; }
        public int SoLuong { get; set; }
        public int Gia { get; set; }
        public int Tong => SoLuong * Gia;
    }
}
