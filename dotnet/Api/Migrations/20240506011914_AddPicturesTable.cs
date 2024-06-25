using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPicturesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CellphoneNumber",
                table: "People",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CityName",
                table: "People",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "People",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Instrument",
                table: "People",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Picture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    PictureUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picture", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_People_Email",
                table: "People",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_People_Id",
                table: "People",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Picture");

            migrationBuilder.DropIndex(
                name: "IX_People_Email",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_Id",
                table: "People");

            migrationBuilder.DropColumn(
                name: "CellphoneNumber",
                table: "People");

            migrationBuilder.DropColumn(
                name: "CityName",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Instrument",
                table: "People");
        }
    }
}
