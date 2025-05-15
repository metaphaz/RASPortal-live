using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RASPortal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEventMediaToArrayOfPaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventMedia",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventMedia",
                table: "Events");
        }
    }
}
