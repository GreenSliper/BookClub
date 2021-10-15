using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DAL.Migrations
{
    public partial class Discussions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClubDiscussion",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClubID = table.Column<int>(type: "integer", nullable: true),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubDiscussion", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubDiscussion_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClubDiscussion_Clubs_ClubID",
                        column: x => x.ClubID,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClubDiscussionBook",
                columns: table => new
                {
                    DiscussionID = table.Column<int>(type: "integer", nullable: false),
                    BookID = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubDiscussionBook", x => new { x.BookID, x.DiscussionID });
                    table.ForeignKey(
                        name: "FK_ClubDiscussionBook_Books_BookID",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubDiscussionBook_ClubDiscussion_DiscussionID",
                        column: x => x.DiscussionID,
                        principalTable: "ClubDiscussion",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubDiscussion_ClubID",
                table: "ClubDiscussion",
                column: "ClubID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubDiscussion_CreatorId",
                table: "ClubDiscussion",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubDiscussionBook_DiscussionID",
                table: "ClubDiscussionBook",
                column: "DiscussionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubDiscussionBook");

            migrationBuilder.DropTable(
                name: "ClubDiscussion");
        }
    }
}
