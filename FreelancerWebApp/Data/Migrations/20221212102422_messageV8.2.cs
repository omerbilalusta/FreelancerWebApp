using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerWebApp.Data.Migrations
{
    public partial class messageV82 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inbox_participants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: true),
                    inboxId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inbox_participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inbox_participants_inbox_inboxId",
                        column: x => x.inboxId,
                        principalTable: "inbox",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_inbox_participants_user_userId",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    message_text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    inboxId = table.Column<int>(type: "int", nullable: true),
                    userId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_message_inbox_inboxId",
                        column: x => x.inboxId,
                        principalTable: "inbox",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_message_user_userId",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_inbox_participants_inboxId",
                table: "inbox_participants",
                column: "inboxId");

            migrationBuilder.CreateIndex(
                name: "IX_inbox_participants_userId",
                table: "inbox_participants",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_message_inboxId",
                table: "message",
                column: "inboxId");

            migrationBuilder.CreateIndex(
                name: "IX_message_userId",
                table: "message",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inbox_participants");

            migrationBuilder.DropTable(
                name: "message");
        }
    }
}
