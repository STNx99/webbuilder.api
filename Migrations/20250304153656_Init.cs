using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webbuilder.api.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Elements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false),
                    Styles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    Src = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Href = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProjectId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Elements_Elements_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Elements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Elements_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Description", "Name", "OwnerId" },
                values: new object[] { "1", "A sample project with elements", "Sample Project", "1" });

            migrationBuilder.InsertData(
                table: "Elements",
                columns: new[] { "Id", "Content", "Href", "IsSelected", "ParentId", "ProjectId", "Src", "Styles", "Type", "X", "Y" },
                values: new object[,]
                {
                    { "1", "Hello, World!", null, false, null, "1", null, "{\"color\":\"red\",\"font-size\":\"12px\"}", "Text", 10, 20 },
                    { "2", null, null, false, null, "1", null, "{\"border\":\"1px solid black\"}", "Frame", 50, 100 },
                    { "3", "Nested Text", null, false, "2", "1", null, "{\"color\":\"blue\"}", "Text", 10, 10 },
                    { "4", null, null, false, "2", "1", null, "{\"background\":\"yellow\"}", "Frame", 20, 20 },
                    { "5", "Deeply Nested Text", null, false, "4", "1", null, "{\"font-weight\":\"bold\"}", "Text", 5, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Elements_ParentId",
                table: "Elements",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_ProjectId",
                table: "Elements",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Elements");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
