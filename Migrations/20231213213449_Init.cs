using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Project.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "interest",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interest", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "InterestPerson",
                columns: table => new
                {
                    Interestsid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Peopleid = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestPerson", x => new { x.Interestsid, x.Peopleid });
                    table.ForeignKey(
                        name: "FK_InterestPerson_interest_Interestsid",
                        column: x => x.Interestsid,
                        principalTable: "interest",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterestPerson_person_Peopleid",
                        column: x => x.Peopleid,
                        principalTable: "person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterestsLink",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WebLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Interestid = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Personid = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestsLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestsLink_interest_Interestid",
                        column: x => x.Interestid,
                        principalTable: "interest",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_InterestsLink_person_Personid",
                        column: x => x.Personid,
                        principalTable: "person",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterestPerson_Peopleid",
                table: "InterestPerson",
                column: "Peopleid");

            migrationBuilder.CreateIndex(
                name: "IX_InterestsLink_Interestid",
                table: "InterestsLink",
                column: "Interestid");

            migrationBuilder.CreateIndex(
                name: "IX_InterestsLink_Personid",
                table: "InterestsLink",
                column: "Personid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterestPerson");

            migrationBuilder.DropTable(
                name: "InterestsLink");

            migrationBuilder.DropTable(
                name: "interest");

            migrationBuilder.DropTable(
                name: "person");
        }
    }
}
