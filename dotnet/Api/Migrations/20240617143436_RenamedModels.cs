using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class RenamedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BandModelPersonModel");

            migrationBuilder.CreateTable(
                name: "BandPerson",
                columns: table => new
                {
                    BandsId = table.Column<int>(type: "integer", nullable: false),
                    MembersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandPerson", x => new { x.BandsId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_BandPerson_Band_BandsId",
                        column: x => x.BandsId,
                        principalTable: "Band",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BandPerson_People_MembersId",
                        column: x => x.MembersId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BandPerson_MembersId",
                table: "BandPerson",
                column: "MembersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BandPerson");

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
    }
}
