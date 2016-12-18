using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeDAL.Context
{
    public class RankingPlace
    {
        public int RankingPlaceId { get; set; }

        public int Points { get; set; }

        public DateTime TimeOfResult { get; set; }
            
        public virtual Speed Speed { get; set; }

        public virtual GameMode GameMode { get; set; }

        public virtual TimeDuration TimeDuration { get; set; }
    }
}
