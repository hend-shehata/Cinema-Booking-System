using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cinema_Booking_System.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "CategoryId", "CinemaId", "DateTime", "Des", "MainImg", "Name", "Price", "Status" },
                values: new object[,]
                {
                    { 5, 1, 2, new DateTime(2026, 11, 21, 19, 0, 0, 0, DateTimeKind.Unspecified), "A skilled driver takes on a dangerous mission.", "movie5.jpg", "Fast Escape", 160m, true },
                    { 6, 2, 3, new DateTime(2026, 11, 23, 17, 30, 0, 0, DateTimeKind.Unspecified), "Two neighbors get into hilarious situations.", "movie6.jpg", "Funny Neighbors", 110m, true },
                    { 7, 3, 1, new DateTime(2026, 11, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), "A family struggles to overcome life's challenges.", "movie7.jpg", "Broken Dreams", 130m, true },
                    { 8, 4, 3, new DateTime(2026, 11, 26, 22, 0, 0, 0, DateTimeKind.Unspecified), "Friends discover a terrifying secret in an old house.", "movie8.jpg", "The Haunted House", 125m, true },
                    { 9, 5, 2, new DateTime(2026, 11, 27, 20, 0, 0, 0, DateTimeKind.Unspecified), "Astronauts explore a mysterious distant planet.", "movie9.jpg", "Beyond the Stars", 190m, true },
                    { 10, 1, 1, new DateTime(2026, 11, 28, 21, 0, 0, 0, DateTimeKind.Unspecified), "A secret agent races against time to save the city.", "movie10.jpg", "Final Mission", 170m, true },
                    { 11, 2, 2, new DateTime(2026, 11, 29, 16, 0, 0, 0, DateTimeKind.Unspecified), "A family vacation turns into a comedy of errors.", "movie11.jpg", "Crazy Vacation", 105m, true },
                    { 12, 3, 3, new DateTime(2026, 11, 30, 19, 30, 0, 0, DateTimeKind.Unspecified), "A forgotten letter changes two lives forever.", "movie12.jpg", "The Last Letter", 140m, true },
                    { 13, 4, 1, new DateTime(2026, 12, 2, 22, 0, 0, 0, DateTimeKind.Unspecified), "A group of friends face an unexplained mystery.", "movie13.jpg", "Midnight Fear", 135m, false },
                    { 14, 5, 2, new DateTime(2026, 12, 3, 20, 30, 0, 0, DateTimeKind.Unspecified), "Scientists discover a way to travel through time.", "movie14.jpg", "Time Travelers", 200m, true },
                    { 15, 1, 3, new DateTime(2026, 12, 4, 21, 0, 0, 0, DateTimeKind.Unspecified), "A team fights to protect their city from a powerful enemy.", "movie15.jpg", "The Final Stand", 175m, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 15);
        }
    }
}
