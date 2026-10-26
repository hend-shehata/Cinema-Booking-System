namespace Cinema_Booking_System.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Movie> Movies { get; set; } = new List<Movie>(); //new()
    }
}
