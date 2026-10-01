using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlow.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixProjectModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectMembers",
                table: "ProjectMembers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectMembers_ProjectId_UserId",
                table: "ProjectMembers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectMembers",
                table: "ProjectMembers",
                columns: new[] { "ProjectId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectMembers",
                table: "ProjectMembers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectMembers",
                table: "ProjectMembers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProjectId_UserId",
                table: "ProjectMembers",
                columns: new[] { "ProjectId", "UserId" },
                unique: true);
        }
    }
}
