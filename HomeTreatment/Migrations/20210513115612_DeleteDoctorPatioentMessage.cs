using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeTreatment.Migrations
{
    public partial class DeleteDoctorPatioentMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DDoctorPatientMessages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
