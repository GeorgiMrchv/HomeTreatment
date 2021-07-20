using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeTreatment.Data.Migrations
{
    public partial class AllowMultipleMessagesForPatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DoctorPatientMessages_PatientId",
                table: "DoctorPatientMessages");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorPatientMessages_PatientId",
                table: "DoctorPatientMessages",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DoctorPatientMessages_PatientId",
                table: "DoctorPatientMessages");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorPatientMessages_PatientId",
                table: "DoctorPatientMessages",
                column: "PatientId",
                unique: true);
        }
    }
}
