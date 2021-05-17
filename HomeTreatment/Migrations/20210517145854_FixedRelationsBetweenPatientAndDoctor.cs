using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeTreatment.Migrations
{
    public partial class FixedRelationsBetweenPatientAndDoctor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "DoctorPatientMessages",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorPatientMessages_DoctorId",
                table: "DoctorPatientMessages",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorPatientMessages_Doctors_DoctorId",
                table: "DoctorPatientMessages",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorPatientMessages_Doctors_DoctorId",
                table: "DoctorPatientMessages");

            migrationBuilder.DropIndex(
                name: "IX_DoctorPatientMessages_DoctorId",
                table: "DoctorPatientMessages");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "DoctorPatientMessages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
