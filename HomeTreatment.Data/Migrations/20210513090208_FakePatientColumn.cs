using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeTreatment.Data.Migrations
{
    public partial class FakePatientColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatienttId",
                table: "DoctorPatientMessages",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Patientts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    AttentionLevel = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patientts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorPatientMessages_PatienttId",
                table: "DoctorPatientMessages",
                column: "PatienttId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorPatientMessages_Patientts_PatienttId",
                table: "DoctorPatientMessages",
                column: "PatienttId",
                principalTable: "Patientts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorPatientMessages_Patientts_PatienttId",
                table: "DoctorPatientMessages");

            migrationBuilder.DropTable(
                name: "Patientts");

            migrationBuilder.DropIndex(
                name: "IX_DoctorPatientMessages_PatienttId",
                table: "DoctorPatientMessages");

            migrationBuilder.DropColumn(
                name: "PatienttId",
                table: "DoctorPatientMessages");
        }
    }
}
