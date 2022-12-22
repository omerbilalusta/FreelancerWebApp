using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerWebApp.Data.Migrations
{
    public partial class thirdStage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Deliver_File_Path",
                table: "job",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deliver_File_Path",
                table: "job");
        }
    }
}
