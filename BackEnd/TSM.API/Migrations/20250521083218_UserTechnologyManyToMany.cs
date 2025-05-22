using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSM.API.Migrations
{
    /// <inheritdoc />
    public partial class UserTechnologyManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TechnologySkills",
                newName: "SkillId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TeamMembers",
                newName: "MemberId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Projects",
                newName: "ProjectId");

            migrationBuilder.AlterColumn<decimal>(
                name: "YearsOfExperience",
                table: "TechnologySkills",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TechnologyId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserTechnologies",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TechnologyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTechnologies", x => new { x.UserId, x.TechnologyId });
                    table.ForeignKey(
                        name: "FK_UserTechnologies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTechnologies_Technologies_TechnologyId",
                        column: x => x.TechnologyId,
                        principalTable: "Technologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TechnologyId",
                table: "AspNetUsers",
                column: "TechnologyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTechnologies_TechnologyId",
                table: "UserTechnologies",
                column: "TechnologyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Technologies_TechnologyId",
                table: "AspNetUsers",
                column: "TechnologyId",
                principalTable: "Technologies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Technologies_TechnologyId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserTechnologies");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TechnologyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TechnologyId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "SkillId",
                table: "TechnologySkills",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "TeamMembers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Projects",
                newName: "Id");

            migrationBuilder.AlterColumn<decimal>(
                name: "YearsOfExperience",
                table: "TechnologySkills",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldNullable: true);
        }
    }
}
