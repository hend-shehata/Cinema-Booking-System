namespace Cinema_Booking_System.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Img { get; set; }

        public List<Movie> Movies { get; set; } = new();
    }
}
