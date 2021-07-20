using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeTreatment.Data.Migrations
{
    public partial class AddedFirstVisitColumnToPatients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFirstVisit",
                table: "Patients",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFirstVisit",
                table: "Patients");
        }
    }
}
