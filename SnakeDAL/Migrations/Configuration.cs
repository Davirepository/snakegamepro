using SnakeDAL.Context;

namespace SnakeDAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SnakeDAL.Context.SnakeContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SnakeDAL.Context.SnakeContext context)
        {
            if (context.Database.Exists())
            {
                context.Database.Delete();
                context.Database.Create();
            }

            context.GameModes.AddOrUpdate(gm => gm.GameModeName,
                new GameMode() {GameModeName = "Classic"},
                new GameMode() { GameModeName = "Time"},
                new GameMode() { GameModeName = "Extra"}
                );

            context.Speeds.AddOrUpdate(s => s.SpeedName,
                new Speed() { SpeedName = "Normal"},
                new Speed() { SpeedName = "High"},
                new Speed() { SpeedName = "Extreme"}
                );

            context.TimeDurations.AddOrUpdate(td => td.TimeDurationName,
                new TimeDuration() { TimeDurationName = "One minute"},
                new TimeDuration() { TimeDurationName = "Two minutes"},
                new TimeDuration() { TimeDurationName = "Three minutes"},
                new TimeDuration() { TimeDurationName = "None"}
                );
        }
    }
}
