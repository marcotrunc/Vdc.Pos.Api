using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vdc.Pos.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NewNameOtpTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Otp_Users_UserId",
                table: "Otp");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Otp",
                table: "Otp");

            migrationBuilder.RenameTable(
                name: "Otp",
                newName: "Otps");

            migrationBuilder.RenameIndex(
                name: "IX_Otp_UserId",
                table: "Otps",
                newName: "IX_Otps_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Otps",
                table: "Otps",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Otps_Users_UserId",
                table: "Otps",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Otps_Users_UserId",
                table: "Otps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Otps",
                table: "Otps");

            migrationBuilder.RenameTable(
                name: "Otps",
                newName: "Otp");

            migrationBuilder.RenameIndex(
                name: "IX_Otps_UserId",
                table: "Otp",
                newName: "IX_Otp_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Otp",
                table: "Otp",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Otp_Users_UserId",
                table: "Otp",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
