using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventPlannerRSVPTracker.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddReservedPaxProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReservedPax",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservedPax",
                table: "Events");
        }
    }
}
