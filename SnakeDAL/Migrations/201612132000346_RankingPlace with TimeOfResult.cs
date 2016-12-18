namespace SnakeDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RankingPlacewithTimeOfResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RankingPlaces", "TimeOfResult", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RankingPlaces", "TimeOfResult");
        }
    }
}
