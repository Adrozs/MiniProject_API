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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterestLink",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WebLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InterestsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestLink_interest_InterestsId",
                        column: x => x.InterestsId,
                        principalTable: "interest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterestLink_person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InterestPerson",
                columns: table => new
                {
                    InterestsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PeopleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestPerson", x => new { x.InterestsId, x.PeopleId });
                    table.ForeignKey(
                        name: "FK_InterestPerson_interest_InterestsId",
                        column: x => x.InterestsId,
                        principalTable: "interest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterestPerson_person_PeopleId",
                        column: x => x.PeopleId,
                        principalTable: "person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterestLink_InterestsId",
                table: "InterestLink",
                column: "InterestsId");

            migrationBuilder.CreateIndex(
                name: "IX_InterestLink_PersonId",
                table: "InterestLink",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_InterestPerson_PeopleId",
                table: "InterestPerson",
                column: "PeopleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterestLink");

            migrationBuilder.DropTable(
                name: "InterestPerson");

            migrationBuilder.DropTable(
                name: "interest");

            migrationBuilder.DropTable(
                name: "person");
        }
    }
}
