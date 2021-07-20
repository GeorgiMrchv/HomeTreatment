using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeTreatment.Data.Migrations
{
    public partial class FakeFakePatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientttId",
                table: "DoctorPatientMessages",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Patienttts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    AttentionLevel = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    DoctorId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patienttts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorPatientMessages_PatientttId",
                table: "DoctorPatientMessages",
                column: "PatientttId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorPatientMessages_Patienttts_PatientttId",
                table: "DoctorPatientMessages",
                column: "PatientttId",
                principalTable: "Patienttts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorPatientMessages_Patienttts_PatientttId",
                table: "DoctorPatientMessages");

            migrationBuilder.DropTable(
                name: "Patienttts");

            migrationBuilder.DropIndex(
                name: "IX_DoctorPatientMessages_PatientttId",
                table: "DoctorPatientMessages");

            migrationBuilder.DropColumn(
                name: "PatientttId",
                table: "DoctorPatientMessages");
        }
    }
}
