using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeDAL.Context;

namespace SnakeDAL.Initializer
{
    public class SnakeDbInitializer : CreateDatabaseIfNotExists<SnakeDAL.Context.SnakeContext>
    {
        protected override void Seed(SnakeContext context)
        {
            if (context.Database.Exists())
            {
                context.Database.Delete();
                context.Database.Create();
            }

            context.GameModes.AddOrUpdate(gm => gm.GameModeName,
                new GameMode() { GameModeName = "Classic" },
                new GameMode() { GameModeName = "Time" },
                new GameMode() { GameModeName = "Extra" }
                );

            context.Speeds.AddOrUpdate(s => s.SpeedName,
                new Speed() { SpeedName = "Normal" },
                new Speed() { SpeedName = "High" },
                new Speed() { SpeedName = "Extreme" }
                );

            context.TimeDurations.AddOrUpdate(td => td.TimeDurationName,
                new TimeDuration() { TimeDurationName = "One minute" },
                new TimeDuration() { TimeDurationName = "Two minutes" },
                new TimeDuration() { TimeDurationName = "Three minutes" },
                new TimeDuration() { TimeDurationName = "None" }
                );
        }
    }
}
