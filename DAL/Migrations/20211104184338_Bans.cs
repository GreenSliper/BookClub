using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Bans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ban",
                columns: table => new
                {
                    ClubID = table.Column<int>(type: "integer", nullable: false),
                    BannedUserID = table.Column<string>(type: "text", nullable: false),
                    BannedByID = table.Column<string>(type: "text", nullable: true),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ban", x => new { x.ClubID, x.BannedUserID });
                    table.ForeignKey(
                        name: "FK_Ban_AspNetUsers_BannedByID",
                        column: x => x.BannedByID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ban_AspNetUsers_BannedUserID",
                        column: x => x.BannedUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ban_Clubs_ClubID",
                        column: x => x.ClubID,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ban_BannedByID",
                table: "Ban",
                column: "BannedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Ban_BannedUserID",
                table: "Ban",
                column: "BannedUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ban");
        }
    }
}
