namespace SnakeDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstSnakeDbschema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameModes",
                c => new
                    {
                        GameModeId = c.Int(nullable: false, identity: true),
                        GameModeName = c.String(),
                    })
                .PrimaryKey(t => t.GameModeId);
            
            CreateTable(
                "dbo.RankingPlaces",
                c => new
                    {
                        RankingPlaceId = c.Int(nullable: false, identity: true),
                        Points = c.Int(nullable: false),
                        GameMode_GameModeId = c.Int(),
                        Speed_SpeedId = c.Int(),
                        TimeDuration_TimeDurationId = c.Int(),
                    })
                .PrimaryKey(t => t.RankingPlaceId)
                .ForeignKey("dbo.GameModes", t => t.GameMode_GameModeId)
                .ForeignKey("dbo.Speeds", t => t.Speed_SpeedId)
                .ForeignKey("dbo.TimeDurations", t => t.TimeDuration_TimeDurationId)
                .Index(t => t.GameMode_GameModeId)
                .Index(t => t.Speed_SpeedId)
                .Index(t => t.TimeDuration_TimeDurationId);
            
            CreateTable(
                "dbo.Speeds",
                c => new
                    {
                        SpeedId = c.Int(nullable: false, identity: true),
                        SpeedName = c.String(),
                    })
                .PrimaryKey(t => t.SpeedId);
            
            CreateTable(
                "dbo.TimeDurations",
                c => new
                    {
                        TimeDurationId = c.Int(nullable: false, identity: true),
                        TimeDurationName = c.String(),
                    })
                .PrimaryKey(t => t.TimeDurationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RankingPlaces", "TimeDuration_TimeDurationId", "dbo.TimeDurations");
            DropForeignKey("dbo.RankingPlaces", "Speed_SpeedId", "dbo.Speeds");
            DropForeignKey("dbo.RankingPlaces", "GameMode_GameModeId", "dbo.GameModes");
            DropIndex("dbo.RankingPlaces", new[] { "TimeDuration_TimeDurationId" });
            DropIndex("dbo.RankingPlaces", new[] { "Speed_SpeedId" });
            DropIndex("dbo.RankingPlaces", new[] { "GameMode_GameModeId" });
            DropTable("dbo.TimeDurations");
            DropTable("dbo.Speeds");
            DropTable("dbo.RankingPlaces");
            DropTable("dbo.GameModes");
        }
    }
}
