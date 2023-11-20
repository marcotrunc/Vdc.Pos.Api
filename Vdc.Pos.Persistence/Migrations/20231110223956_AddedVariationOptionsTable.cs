using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vdc.Pos.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedVariationOptionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Variations_Categories_ParentCategoryId",
                table: "Variations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Variations",
                table: "Variations");

            migrationBuilder.RenameTable(
                name: "Variations",
                newName: "Variation");

            migrationBuilder.RenameIndex(
                name: "IX_Variations_ParentCategoryId",
                table: "Variation",
                newName: "IX_Variation_ParentCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Variation",
                table: "Variation",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "VariationOption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VariationId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariationOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariationOption_Variation_VariationId",
                        column: x => x.VariationId,
                        principalTable: "Variation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VariationOption_VariationId",
                table: "VariationOption",
                column: "VariationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Variation_Categories_ParentCategoryId",
                table: "Variation",
                column: "ParentCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Variation_Categories_ParentCategoryId",
                table: "Variation");

            migrationBuilder.DropTable(
                name: "VariationOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Variation",
                table: "Variation");

            migrationBuilder.RenameTable(
                name: "Variation",
                newName: "Variations");

            migrationBuilder.RenameIndex(
                name: "IX_Variation_ParentCategoryId",
                table: "Variations",
                newName: "IX_Variations_ParentCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Variations",
                table: "Variations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Variations_Categories_ParentCategoryId",
                table: "Variations",
                column: "ParentCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
