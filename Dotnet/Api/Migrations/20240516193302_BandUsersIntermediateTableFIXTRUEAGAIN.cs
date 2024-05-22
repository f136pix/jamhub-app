using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class BandUsersIntermediateTableFIXTRUEAGAIN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Band",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Band_CreatorId",
                table: "Band",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Band_People_CreatorId",
                table: "Band",
                column: "CreatorId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Band_People_CreatorId",
                table: "Band");

            migrationBuilder.DropIndex(
                name: "IX_Band_CreatorId",
                table: "Band");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Band");
        }
    }
}
