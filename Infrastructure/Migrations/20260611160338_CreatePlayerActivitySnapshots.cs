using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreatePlayerActivitySnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DO $$ 
            BEGIN 
                IF EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'player_activity_snapshots') THEN
                    
                    IF NOT EXISTS (SELECT FROM information_schema.columns WHERE table_name = 'player_activity_snapshots' AND column_name = 'ClanMemberTag') THEN
                        ALTER TABLE player_activity_snapshots ADD COLUMN ""ClanMemberTag"" character varying(10) NULL;
                    END IF;

                    IF NOT EXISTS (SELECT FROM pg_indexes WHERE tablename = 'player_activity_snapshots' AND indexname = 'IX_player_activity_snapshots_ClanMemberTag') THEN
                        CREATE INDEX ""IX_player_activity_snapshots_ClanMemberTag"" ON player_activity_snapshots (""ClanMemberTag"");
                    END IF;

                    IF NOT EXISTS (SELECT FROM information_schema.table_constraints WHERE constraint_name = 'FK_player_activity_snapshots_clan_members_ClanMemberTag') THEN
                        ALTER TABLE player_activity_snapshots 
                        ADD CONSTRAINT ""FK_player_activity_snapshots_clan_members_ClanMemberTag"" 
                        FOREIGN KEY (""ClanMemberTag"") REFERENCES clan_members (""Tag"");
                    END IF;

                END IF;
            END $$;
        ");
        }
    }
}
