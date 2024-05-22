using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class BandUsersIntermediateTableFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BandUserModel_Band_BandId",
                table: "BandUserModel");

            migrationBuilder.DropForeignKey(
                name: "FK_BandUserModel_People_PersonId",
                table: "BandUserModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BandUserModel",
                table: "BandUserModel");

            migrationBuilder.RenameTable(
                name: "BandUserModel",
                newName: "BandUser");

            migrationBuilder.RenameIndex(
                name: "IX_BandUserModel_BandId_PersonId",
                table: "BandUser",
                newName: "IX_BandUser_BandId_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BandUser",
                table: "BandUser",
                columns: new[] { "PersonId", "BandId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BandUser_Band_BandId",
                table: "BandUser",
                column: "BandId",
                principalTable: "Band",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BandUser_People_PersonId",
                table: "BandUser",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BandUser_Band_BandId",
                table: "BandUser");

            migrationBuilder.DropForeignKey(
                name: "FK_BandUser_People_PersonId",
                table: "BandUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BandUser",
                table: "BandUser");

            migrationBuilder.RenameTable(
                name: "BandUser",
                newName: "BandUserModel");

            migrationBuilder.RenameIndex(
                name: "IX_BandUser_BandId_PersonId",
                table: "BandUserModel",
                newName: "IX_BandUserModel_BandId_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BandUserModel",
                table: "BandUserModel",
                columns: new[] { "PersonId", "BandId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BandUserModel_Band_BandId",
                table: "BandUserModel",
                column: "BandId",
                principalTable: "Band",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BandUserModel_People_PersonId",
                table: "BandUserModel",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
