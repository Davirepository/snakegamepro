using System;

namespace SnakeEngine
{
    public class TimerEventArgs : EventArgs
    {
        public TimeSpan RestOfTime { get; set; }
    }   
}
