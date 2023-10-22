using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vdc.Pos.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOtpsTableIsUsedColumdAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "Otps",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "Otps");
        }
    }
}
