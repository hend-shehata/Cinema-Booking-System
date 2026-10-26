using Cinema_Booking_System.Models;

namespace Cinema_Booking_System.ViewModels
{
    public class MovieListViewModel
    {
        public List<Movie> Movies { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<Cinema> Cinemas { get; set; } = new();

        // الفلاتر الحالية
        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public int? CinemaId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // Pagination
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}