using SnakeDAL.Initializer;

namespace SnakeDAL.Context
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class SnakeContext : DbContext
    {
        public SnakeContext()
            : base("name=SnakeContext")
        {
            Database.SetInitializer(new SnakeDbInitializer());
        }

        public virtual DbSet<GameMode> GameModes { get; set; }
        public virtual DbSet<Speed> Speeds { get; set; }
        public virtual DbSet<TimeDuration> TimeDurations { get; set; }
        public virtual DbSet<RankingPlace> RankingPlaces { get; set; }
    }
}