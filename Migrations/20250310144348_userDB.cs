using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webbuilder.api.Migrations
{
    /// <inheritdoc />
    public partial class userDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClerkUserId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClerkUserId",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
