using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Project.Migrations
{
    public partial class addedNavProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterestLink_interest_InterestId",
                table: "InterestLink");

            migrationBuilder.DropIndex(
                name: "IX_InterestLink_InterestId",
                table: "InterestLink");

            migrationBuilder.DropColumn(
                name: "InterestId",
                table: "InterestLink");

            migrationBuilder.AddColumn<string>(
                name: "InterestsId",
                table: "InterestLink",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_InterestLink_InterestsId",
                table: "InterestLink",
                column: "InterestsId");

            migrationBuilder.AddForeignKey(
                name: "FK_InterestLink_interest_InterestsId",
                table: "InterestLink",
                column: "InterestsId",
                principalTable: "interest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterestLink_interest_InterestsId",
                table: "InterestLink");

            migrationBuilder.DropIndex(
                name: "IX_InterestLink_InterestsId",
                table: "InterestLink");

            migrationBuilder.DropColumn(
                name: "InterestsId",
                table: "InterestLink");

            migrationBuilder.AddColumn<string>(
                name: "InterestId",
                table: "InterestLink",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterestLink_InterestId",
                table: "InterestLink",
                column: "InterestId");

            migrationBuilder.AddForeignKey(
                name: "FK_InterestLink_interest_InterestId",
                table: "InterestLink",
                column: "InterestId",
                principalTable: "interest",
                principalColumn: "Id");
        }
    }
}
