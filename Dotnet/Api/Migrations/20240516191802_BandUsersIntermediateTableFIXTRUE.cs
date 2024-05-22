using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class BandUsersIntermediateTableFIXTRUE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BandUser");

            migrationBuilder.CreateTable(
                name: "BandModelPersonModel",
                columns: table => new
                {
                    BandsId = table.Column<int>(type: "integer", nullable: false),
                    MembersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandModelPersonModel", x => new { x.BandsId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_BandModelPersonModel_Band_BandsId",
                        column: x => x.BandsId,
                        principalTable: "Band",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BandModelPersonModel_People_MembersId",
                        column: x => x.MembersId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BandModelPersonModel_MembersId",
                table: "BandModelPersonModel",
                column: "MembersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BandModelPersonModel");

            migrationBuilder.CreateTable(
                name: "BandUser",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    BandId = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandUser", x => new { x.PersonId, x.BandId });
                    table.ForeignKey(
                        name: "FK_BandUser_Band_BandId",
                        column: x => x.BandId,
                        principalTable: "Band",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BandUser_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BandUser_BandId_PersonId",
                table: "BandUser",
                columns: new[] { "BandId", "PersonId" },
                unique: true);
        }
    }
}
