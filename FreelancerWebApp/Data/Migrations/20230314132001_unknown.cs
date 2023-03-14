using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerWebApp.Data.Migrations
{
    public partial class unknown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comment_JobId",
                table: "Comment");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_JobId",
                table: "Comment",
                column: "JobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comment_JobId",
                table: "Comment");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_JobId",
                table: "Comment",
                column: "JobId",
                unique: true);
        }
    }
}
