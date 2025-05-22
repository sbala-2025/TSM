using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSM.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToTechnologyStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TechnologyStatus",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnologyStatus_UserId",
                table: "TechnologyStatus",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologyStatus_AspNetUsers_UserId",
                table: "TechnologyStatus",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnologyStatus_AspNetUsers_UserId",
                table: "TechnologyStatus");

            migrationBuilder.DropIndex(
                name: "IX_TechnologyStatus_UserId",
                table: "TechnologyStatus");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TechnologyStatus");
        }
    }
}
