using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormaaS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string sql = """
                UPDATE
                    "FormFields"
                SET
                    "DetailsJsonColumn" = jsonb_set(
                    '{}'::jsonb,
                    '{TextInput}',
                    "DetailsJsonColumn"
                    )
                """;

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
