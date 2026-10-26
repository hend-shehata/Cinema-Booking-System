using Cinema_Booking_System.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cinema_Booking_System.ViewModels
{
    public class MovieFormViewModel
    {
        public Movie Movie { get; set; } = new();
        public List<int> SelectedActorIds { get; set; } = new();

        public List<SelectListItem> Categories { get; set; } = new();
        public List<SelectListItem> Cinemas { get; set; } = new();
        public List<SelectListItem> Actors { get; set; } = new();
    }
}