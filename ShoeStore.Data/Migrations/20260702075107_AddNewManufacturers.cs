using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShoeStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewManufacturers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Balenciaga");

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "New Rock");

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Buffalo");

            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 6, "Puma" },
                    { 7, "Reebok" },
                    { 8, "New Balance" },
                    { 9, "Dr. Martens" },
                    { 10, "Converse" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Puma");

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Reebok");

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "New Balance");
        }
    }
}
