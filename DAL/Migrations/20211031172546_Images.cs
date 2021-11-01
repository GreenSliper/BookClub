using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DAL.Migrations
{
    public partial class Images : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvatarImageID",
                table: "Clubs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DBImage",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImageData = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBImage", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_AvatarImageID",
                table: "Clubs",
                column: "AvatarImageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_DBImage_AvatarImageID",
                table: "Clubs",
                column: "AvatarImageID",
                principalTable: "DBImage",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_DBImage_AvatarImageID",
                table: "Clubs");

            migrationBuilder.DropTable(
                name: "DBImage");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_AvatarImageID",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "AvatarImageID",
                table: "Clubs");
        }
    }
}
