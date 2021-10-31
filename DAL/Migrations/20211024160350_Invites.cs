using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Invites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClubInvite",
                columns: table => new
                {
                    ClubID = table.Column<int>(type: "integer", nullable: false),
                    ReceiverID = table.Column<string>(type: "text", nullable: false),
                    InviterID = table.Column<string>(type: "text", nullable: true),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    GivenPermissions = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubInvite", x => new { x.ClubID, x.ReceiverID });
                    table.ForeignKey(
                        name: "FK_ClubInvite_AspNetUsers_InviterID",
                        column: x => x.InviterID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClubInvite_AspNetUsers_ReceiverID",
                        column: x => x.ReceiverID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubInvite_Clubs_ClubID",
                        column: x => x.ClubID,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubInvite_InviterID",
                table: "ClubInvite",
                column: "InviterID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubInvite_ReceiverID",
                table: "ClubInvite",
                column: "ReceiverID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubInvite");
        }
    }
}
