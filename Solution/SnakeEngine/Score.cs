using System;

namespace SnakeEngine
{
    public class Score
    {
        public int Points { get; set; }    
    }

    public class TimeScore : Score
    {
        public TimeSpan Time { get; set; }
    }
}
