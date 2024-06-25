using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class BandUsersIntermediateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Band",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Genre = table.Column<string>(type: "text", nullable: true),
                    CityName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Band", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BandUserModel",
                columns: table => new
                {
                    BandId = table.Column<int>(type: "integer", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandUserModel", x => new { x.PersonId, x.BandId });
                    table.ForeignKey(
                        name: "FK_BandUserModel_Band_BandId",
                        column: x => x.BandId,
                        principalTable: "Band",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BandUserModel_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Band_Id",
                table: "Band",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Band_Name",
                table: "Band",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BandUserModel_BandId_PersonId",
                table: "BandUserModel",
                columns: new[] { "BandId", "PersonId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BandUserModel");

            migrationBuilder.DropTable(
                name: "Band");
        }
    }
}
