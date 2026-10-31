using Cinema_Booking_System.Data;
using Cinema_Booking_System.Models;
using Cinema_Booking_System.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking_System.Repositories
{
    public class MovieSubImageRepository
        : Repository<MovieSubImage>, IMovieSubImageRepository
    {
        public MovieSubImageRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public void RemoveRange(IEnumerable<MovieSubImage> movieSubImages)
        {
            _context.RemoveRange(movieSubImages);
        }
    }
}