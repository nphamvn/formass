using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormaaS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMapping2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details",
                table: "FormFields");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "FormFields",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }
    }
}
