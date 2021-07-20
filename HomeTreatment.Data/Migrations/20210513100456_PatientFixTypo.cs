using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeTreatment.Data.Migrations
{
    public partial class PatientFixTypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorPatientMessages_Patientts_PatienttId",
                table: "DoctorPatientMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorPatientMessages_Patienttts_PatientttId",
                table: "DoctorPatientMessages");            

            migrationBuilder.DropIndex(
                name: "IX_DoctorPatientMessages_PatienttId",
                table: "DoctorPatientMessages");

            migrationBuilder.DropIndex(
                name: "IX_DoctorPatientMessages_PatientttId",
                table: "DoctorPatientMessages");

            migrationBuilder.DropColumn(
                name: "PatienttId",
                table: "DoctorPatientMessages");

            migrationBuilder.DropColumn(
                name: "PatientttId",
                table: "DoctorPatientMessages");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "Patients",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "DoctorPatientMessages",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "Patients",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "DoctorPatientMessages",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatienttId",
                table: "DoctorPatientMessages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientttId",
                table: "DoctorPatientMessages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Patientts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttentionLevel = table.Column<bool>(type: "bit", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patientts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patienttts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttentionLevel = table.Column<bool>(type: "bit", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patienttts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorPatientMessages_PatienttId",
                table: "DoctorPatientMessages",
                column: "PatienttId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorPatientMessages_PatientttId",
                table: "DoctorPatientMessages",
                column: "PatientttId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorPatientMessages_Patientts_PatienttId",
                table: "DoctorPatientMessages",
                column: "PatienttId",
                principalTable: "Patientts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorPatientMessages_Patienttts_PatientttId",
                table: "DoctorPatientMessages",
                column: "PatientttId",
                principalTable: "Patienttts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
