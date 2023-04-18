using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtilityComplaints.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addStatusProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Complaints",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Complaints");
        }
    }
}
