using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerWebApp.Data.Migrations
{
    public partial class messageV821 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_message_inbox_inboxId",
                table: "message");

            migrationBuilder.DropForeignKey(
                name: "FK_message_user_userId",
                table: "message");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "message",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "inboxId",
                table: "message",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_message_inbox_inboxId",
                table: "message",
                column: "inboxId",
                principalTable: "inbox",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_message_user_userId",
                table: "message",
                column: "userId",
                principalTable: "user",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_message_inbox_inboxId",
                table: "message");

            migrationBuilder.DropForeignKey(
                name: "FK_message_user_userId",
                table: "message");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "message",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "inboxId",
                table: "message",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_message_inbox_inboxId",
                table: "message",
                column: "inboxId",
                principalTable: "inbox",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_message_user_userId",
                table: "message",
                column: "userId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
