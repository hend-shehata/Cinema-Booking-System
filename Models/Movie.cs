namespace Cinema_Booking_System.Models
{
    public class Movie
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Des { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public DateTime DateTime { get; set; }
        public string MainImg { get; set; } = string.Empty;

        public List<MovieSubImage> SubImages { get; set; } = new();
        public List<Actor> Actors { get; set; } = new();

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; } = null!;
    }
}
