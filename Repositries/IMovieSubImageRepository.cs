using Cinema_Booking_System.Models;

namespace Cinema_Booking_System.Repositories
{
    public interface IMovieSubImageRepository : IRepository<MovieSubImage>
    {
        void RemoveRange(IEnumerable<MovieSubImage> movieSubImages);
    }
}