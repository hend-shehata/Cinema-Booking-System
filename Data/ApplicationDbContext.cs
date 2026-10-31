using Cinema_Booking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking_System.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieSubImage> MovieSubImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action" },
                new Category { Id = 2, Name = "Comedy" },
                new Category { Id = 3, Name = "Drama" },
                new Category { Id = 4, Name = "Horror" },
                new Category { Id = 5, Name = "Sci-Fi" }
            );

            modelBuilder.Entity<Cinema>().HasData(
                new Cinema { Id = 1, Name = "Cairo Festival City", Img = "cinema1.jpg" },
                new Cinema { Id = 2, Name = "City Stars", Img = "cinema2.jpg" },
                new Cinema { Id = 3, Name = "Mall of Egypt", Img = "cinema3.jpg" }
            );

            modelBuilder.Entity<Actor>().HasData(
                new Actor { Id = 1, Name = "Robert Downey Jr.", Img = "actor1.jpg" },
                new Actor { Id = 2, Name = "Scarlett Johansson", Img = "actor2.jpg" },
                new Actor { Id = 3, Name = "Leonardo DiCaprio", Img = "actor3.jpg" },
                new Actor { Id = 4, Name = "Adam Sandler", Img = "actor4.jpg" },
                new Actor { Id = 5, Name = "Emma Stone", Img = "actor5.jpg" }
            );


            modelBuilder.Entity<Movie>().HasData(
                new Movie { Id = 1, Name = "Iron Hero", Des = "A billionaire builds a suit of armor to fight evil.", Price = 150, Status = true, DateTime = new DateTime(2026, 11, 20, 18, 0, 0), MainImg = "movie1.jpg", CategoryId = 1, CinemaId = 1 },
                new Movie { Id = 2, Name = "Laugh Out Loud", Des = "A hilarious comedy about a family road trip.", Price = 100, Status = true, DateTime = new DateTime(2026, 11, 22, 20, 30, 0), MainImg = "movie2.jpg", CategoryId = 2, CinemaId = 2 },
                new Movie { Id = 3, Name = "Dream Heist", Des = "Thieves enter people's dreams to steal secrets.", Price = 180, Status = true, DateTime = new DateTime(2026, 11, 25, 21, 0, 0), MainImg = "movie3.jpg", CategoryId = 5, CinemaId = 1 },
                new Movie { Id = 4, Name = "Dark Night", Des = "A terrifying night in an abandoned house.", Price = 120, Status = false, DateTime = new DateTime(2026, 12, 1, 22, 0, 0), MainImg = "movie4.jpg", CategoryId = 4, CinemaId = 3 },

                new Movie { Id = 5, Name = "Fast Escape", Des = "A skilled driver takes on a dangerous mission.", Price = 160, Status = true, DateTime = new DateTime(2026, 11, 21, 19, 0, 0), MainImg = "movie5.jpg", CategoryId = 1, CinemaId = 2 },
                new Movie { Id = 6, Name = "Funny Neighbors", Des = "Two neighbors get into hilarious situations.", Price = 110, Status = true, DateTime = new DateTime(2026, 11, 23, 17, 30, 0), MainImg = "movie6.jpg", CategoryId = 2, CinemaId = 3 },
                new Movie { Id = 7, Name = "Broken Dreams", Des = "A family struggles to overcome life's challenges.", Price = 130, Status = true, DateTime = new DateTime(2026, 11, 24, 18, 0, 0), MainImg = "movie7.jpg", CategoryId = 3, CinemaId = 1 },
                new Movie { Id = 8, Name = "The Haunted House", Des = "Friends discover a terrifying secret in an old house.", Price = 125, Status = true, DateTime = new DateTime(2026, 11, 26, 22, 0, 0), MainImg = "movie8.jpg", CategoryId = 4, CinemaId = 3 },
                new Movie { Id = 9, Name = "Beyond the Stars", Des = "Astronauts explore a mysterious distant planet.", Price = 190, Status = true, DateTime = new DateTime(2026, 11, 27, 20, 0, 0), MainImg = "movie9.jpg", CategoryId = 5, CinemaId = 2 },
                new Movie { Id = 10, Name = "Final Mission", Des = "A secret agent races against time to save the city.", Price = 170, Status = true, DateTime = new DateTime(2026, 11, 28, 21, 0, 0), MainImg = "movie10.jpg", CategoryId = 1, CinemaId = 1 },
                new Movie { Id = 11, Name = "Crazy Vacation", Des = "A family vacation turns into a comedy of errors.", Price = 105, Status = true, DateTime = new DateTime(2026, 11, 29, 16, 0, 0), MainImg = "movie11.jpg", CategoryId = 2, CinemaId = 2 },
                new Movie { Id = 12, Name = "The Last Letter", Des = "A forgotten letter changes two lives forever.", Price = 140, Status = true, DateTime = new DateTime(2026, 11, 30, 19, 30, 0), MainImg = "movie12.jpg", CategoryId = 3, CinemaId = 3 },
                new Movie { Id = 13, Name = "Midnight Fear", Des = "A group of friends face an unexplained mystery.", Price = 135, Status = false, DateTime = new DateTime(2026, 12, 2, 22, 0, 0), MainImg = "movie13.jpg", CategoryId = 4, CinemaId = 1 },
                new Movie { Id = 14, Name = "Time Travelers", Des = "Scientists discover a way to travel through time.", Price = 200, Status = true, DateTime = new DateTime(2026, 12, 3, 20, 30, 0), MainImg = "movie14.jpg", CategoryId = 5, CinemaId = 2 },
                new Movie { Id = 15, Name = "The Final Stand", Des = "A team fights to protect their city from a powerful enemy.", Price = 175, Status = true, DateTime = new DateTime(2026, 12, 4, 21, 0, 0), MainImg = "movie15.jpg", CategoryId = 1, CinemaId = 3 }
            );


            modelBuilder.Entity<MovieSubImage>().HasData(
                new MovieSubImage { Id = 1, Img = "movie1_a.jpg", MovieId = 1 },
                new MovieSubImage { Id = 2, Img = "movie1_b.jpg", MovieId = 1 },
                new MovieSubImage { Id = 3, Img = "movie2_a.jpg", MovieId = 2 },
                new MovieSubImage { Id = 4, Img = "movie3_a.jpg", MovieId = 3 },
                new MovieSubImage { Id = 5, Img = "movie3_b.jpg", MovieId = 3 }
            );

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Actors)
                .WithMany(a => a.Movies)
                .UsingEntity(j => j.HasData(
                    new { MoviesId = 1, ActorsId = 1 },
                    new { MoviesId = 1, ActorsId = 2 },
                    new { MoviesId = 2, ActorsId = 4 },
                    new { MoviesId = 2, ActorsId = 5 },
                    new { MoviesId = 3, ActorsId = 3 },
                    new { MoviesId = 3, ActorsId = 5 },
                    new { MoviesId = 4, ActorsId = 2 }
                ));
        }
    }
}