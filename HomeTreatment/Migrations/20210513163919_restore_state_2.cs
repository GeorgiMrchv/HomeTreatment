using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeTreatment.Migrations
{
    public partial class restore_state_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Doctors_Id",
                table: "AspNetUsers",
                column: "Id",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
