using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAverageStarsPerAttackToPlayerView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var scriptPath = Path.Combine(
                AppContext.BaseDirectory,
                "Migrations",
                "Scripts",
                "20260606171328_AddAverageStarsPerAttack_Up.sql"
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
            var scriptPath = Path.Combine(
                AppContext.BaseDirectory,
                "Migrations",
                "Scripts",
                "20260606171328_AddAverageStarsPerAttack_Down.sql"
            );

            if (!File.Exists(scriptPath))
            {
                throw new FileNotFoundException($"Migration SQL script not found at target path: {scriptPath}");
            }

            migrationBuilder.Sql(File.ReadAllText(scriptPath));
        }
    }
}