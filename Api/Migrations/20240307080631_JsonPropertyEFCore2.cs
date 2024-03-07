using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormaaS.Migrations
{
    /// <inheritdoc />
    public partial class JsonPropertyEFCore2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details",
                table: "FormFields");

            migrationBuilder.AddColumn<string>(
                name: "Details_JsonString",
                table: "FormFields",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details_JsonString",
                table: "FormFields");

            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "FormFields",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }
    }
}
