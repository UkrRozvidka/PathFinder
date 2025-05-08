using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class PointFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_HikeMembers_HikrMemberId",
                table: "Points");

            migrationBuilder.RenameColumn(
                name: "HikrMemberId",
                table: "Points",
                newName: "HikeMemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Points_HikrMemberId",
                table: "Points",
                newName: "IX_Points_HikeMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_HikeMembers_HikeMemberId",
                table: "Points",
                column: "HikeMemberId",
                principalTable: "HikeMembers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_HikeMembers_HikeMemberId",
                table: "Points");

            migrationBuilder.RenameColumn(
                name: "HikeMemberId",
                table: "Points",
                newName: "HikrMemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Points_HikeMemberId",
                table: "Points",
                newName: "IX_Points_HikrMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_HikeMembers_HikrMemberId",
                table: "Points",
                column: "HikrMemberId",
                principalTable: "HikeMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
