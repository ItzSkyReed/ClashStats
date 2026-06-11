using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OptimizePlayerActivitySnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityScore",
                table: "player_activity_snapshots");

            migrationBuilder.AddColumn<int>(
                name: "ActivityStateMemberInternalId",
                table: "clan_members",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "player_activity_states",
                columns: table => new
                {
                    MemberInternalId = table.Column<int>(type: "integer", nullable: false),
                    ActivityScore = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_activity_states", x => x.MemberInternalId);
                    table.ForeignKey(
                        name: "FK_player_activity_states_clan_members_MemberInternalId",
                        column: x => x.MemberInternalId,
                        principalTable: "clan_members",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_clan_members_ActivityStateMemberInternalId",
                table: "clan_members",
                column: "ActivityStateMemberInternalId");

            migrationBuilder.AddForeignKey(
                name: "FK_clan_members_player_activity_states_ActivityStateMemberInte~",
                table: "clan_members",
                column: "ActivityStateMemberInternalId",
                principalTable: "player_activity_states",
                principalColumn: "MemberInternalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clan_members_player_activity_states_ActivityStateMemberInte~",
                table: "clan_members");

            migrationBuilder.DropTable(
                name: "player_activity_states");

            migrationBuilder.DropIndex(
                name: "IX_clan_members_ActivityStateMemberInternalId",
                table: "clan_members");

            migrationBuilder.DropColumn(
                name: "ActivityStateMemberInternalId",
                table: "clan_members");

            migrationBuilder.AddColumn<long>(
                name: "ActivityScore",
                table: "player_activity_snapshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
