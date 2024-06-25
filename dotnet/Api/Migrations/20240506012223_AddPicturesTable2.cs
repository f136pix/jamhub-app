using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPicturesTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Picture_PersonId",
                table: "Picture",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Picture_People_PersonId",
                table: "Picture",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Picture_People_PersonId",
                table: "Picture");

            migrationBuilder.DropIndex(
                name: "IX_Picture_PersonId",
                table: "Picture");
        }
    }
}
