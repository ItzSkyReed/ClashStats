using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateCWLGroupSummariesView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var scriptPath = Path.Combine(
                AppContext.BaseDirectory,
                "Migrations",
                "Scripts",
                "20260606203630_CreateCWLGroupSummariesView_Up.sql"
            );

            if (!File.Exists(scriptPath))
            {
                throw new FileNotFoundException($"Migration SQL script not found at target path: {scriptPath}");
            }

            migrationBuilder.Sql(File.ReadAllText(scriptPath));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP MATERIALIZED VIEW IF EXISTS \"mv_clan_war_league_group_summaries \";");
        }
    }
}
