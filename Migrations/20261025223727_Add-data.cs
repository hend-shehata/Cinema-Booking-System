using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cinema_Booking_System.Migrations
{
    /// <inheritdoc />
    public partial class Adddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "Id", "Img", "Name" },
                values: new object[,]
                {
                    { 1, "actor1.jpg", "Robert Downey Jr." },
                    { 2, "actor2.jpg", "Scarlett Johansson" },
                    { 3, "actor3.jpg", "Leonardo DiCaprio" },
                    { 4, "actor4.jpg", "Adam Sandler" },
                    { 5, "actor5.jpg", "Emma Stone" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Action" },
                    { 2, "Comedy" },
                    { 3, "Drama" },
                    { 4, "Horror" },
                    { 5, "Sci-Fi" }
                });

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "Img", "Name" },
                values: new object[,]
                {
                    { 1, "cinema1.jpg", "Cairo Festival City" },
                    { 2, "cinema2.jpg", "City Stars" },
                    { 3, "cinema3.jpg", "Mall of Egypt" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "CategoryId", "CinemaId", "DateTime", "Des", "MainImg", "Name", "Price", "Status" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2026, 11, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), "A billionaire builds a suit of armor to fight evil.", "movie1.jpg", "Iron Hero", 150m, true },
                    { 2, 2, 2, new DateTime(2026, 11, 22, 20, 30, 0, 0, DateTimeKind.Unspecified), "A hilarious comedy about a family road trip.", "movie2.jpg", "Laugh Out Loud", 100m, true },
                    { 3, 5, 1, new DateTime(2026, 11, 25, 21, 0, 0, 0, DateTimeKind.Unspecified), "Thieves enter people's dreams to steal secrets.", "movie3.jpg", "Dream Heist", 180m, true },
                    { 4, 4, 3, new DateTime(2026, 12, 1, 22, 0, 0, 0, DateTimeKind.Unspecified), "A terrifying night in an abandoned house.", "movie4.jpg", "Dark Night", 120m, false }
                });

            migrationBuilder.InsertData(
                table: "ActorMovie",
                columns: new[] { "ActorsId", "MoviesId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 2, 4 },
                    { 3, 3 },
                    { 4, 2 },
                    { 5, 2 },
                    { 5, 3 }
                });

            migrationBuilder.InsertData(
                table: "MovieSubImages",
                columns: new[] { "Id", "Img", "MovieId" },
                values: new object[,]
                {
                    { 1, "movie1_a.jpg", 1 },
                    { 2, "movie1_b.jpg", 1 },
                    { 3, "movie2_a.jpg", 2 },
                    { 4, "movie3_a.jpg", 3 },
                    { 5, "movie3_b.jpg", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActorMovie",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "ActorMovie",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "ActorMovie",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "ActorMovie",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "ActorMovie",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "ActorMovie",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "ActorMovie",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MovieSubImages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MovieSubImages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MovieSubImages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MovieSubImages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MovieSubImages",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
