using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class changingtablesnames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Band_People_CreatorId",
                table: "Band");

            migrationBuilder.DropForeignKey(
                name: "FK_BandPerson_Band_BandsId",
                table: "BandPerson");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfirmationToken_People_UserId",
                table: "ConfirmationToken");

            migrationBuilder.DropForeignKey(
                name: "FK_Picture_People_PersonId",
                table: "Picture");

            migrationBuilder.DropTable(
                name: "Blacklist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Picture",
                table: "Picture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConfirmationToken",
                table: "ConfirmationToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Band",
                table: "Band");

            migrationBuilder.RenameTable(
                name: "Picture",
                newName: "Pictures");

            migrationBuilder.RenameTable(
                name: "ConfirmationToken",
                newName: "ConfirmationTokens");

            migrationBuilder.RenameTable(
                name: "Band",
                newName: "Bands");

            migrationBuilder.RenameIndex(
                name: "IX_Picture_PersonId",
                table: "Pictures",
                newName: "IX_Pictures_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_ConfirmationToken_UserId",
                table: "ConfirmationTokens",
                newName: "IX_ConfirmationTokens_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ConfirmationToken_Token",
                table: "ConfirmationTokens",
                newName: "IX_ConfirmationTokens_Token");

            migrationBuilder.RenameIndex(
                name: "IX_Band_Name",
                table: "Bands",
                newName: "IX_Bands_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Band_CreatorId",
                table: "Bands",
                newName: "IX_Bands_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConfirmationTokens",
                table: "ConfirmationTokens",
                column: "Token");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bands",
                table: "Bands",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BlacklistedTokens",
                columns: table => new
                {
                    Jti = table.Column<string>(type: "text", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistedTokens", x => x.Jti);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistedTokens_Jti",
                table: "BlacklistedTokens",
                column: "Jti",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BandPerson_Bands_BandsId",
                table: "BandPerson",
                column: "BandsId",
                principalTable: "Bands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bands_People_CreatorId",
                table: "Bands",
                column: "CreatorId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfirmationTokens_People_UserId",
                table: "ConfirmationTokens",
                column: "UserId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_People_PersonId",
                table: "Pictures",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BandPerson_Bands_BandsId",
                table: "BandPerson");

            migrationBuilder.DropForeignKey(
                name: "FK_Bands_People_CreatorId",
                table: "Bands");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfirmationTokens_People_UserId",
                table: "ConfirmationTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_People_PersonId",
                table: "Pictures");

            migrationBuilder.DropTable(
                name: "BlacklistedTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConfirmationTokens",
                table: "ConfirmationTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bands",
                table: "Bands");

            migrationBuilder.RenameTable(
                name: "Pictures",
                newName: "Picture");

            migrationBuilder.RenameTable(
                name: "ConfirmationTokens",
                newName: "ConfirmationToken");

            migrationBuilder.RenameTable(
                name: "Bands",
                newName: "Band");

            migrationBuilder.RenameIndex(
                name: "IX_Pictures_PersonId",
                table: "Picture",
                newName: "IX_Picture_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_ConfirmationTokens_UserId",
                table: "ConfirmationToken",
                newName: "IX_ConfirmationToken_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ConfirmationTokens_Token",
                table: "ConfirmationToken",
                newName: "IX_ConfirmationToken_Token");

            migrationBuilder.RenameIndex(
                name: "IX_Bands_Name",
                table: "Band",
                newName: "IX_Band_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Bands_CreatorId",
                table: "Band",
                newName: "IX_Band_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Picture",
                table: "Picture",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConfirmationToken",
                table: "ConfirmationToken",
                column: "Token");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Band",
                table: "Band",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Blacklist",
                columns: table => new
                {
                    Jti = table.Column<string>(type: "text", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blacklist", x => x.Jti);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blacklist_Jti",
                table: "Blacklist",
                column: "Jti",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Band_People_CreatorId",
                table: "Band",
                column: "CreatorId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BandPerson_Band_BandsId",
                table: "BandPerson",
                column: "BandsId",
                principalTable: "Band",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfirmationToken_People_UserId",
                table: "ConfirmationToken",
                column: "UserId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Picture_People_PersonId",
                table: "Picture",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
