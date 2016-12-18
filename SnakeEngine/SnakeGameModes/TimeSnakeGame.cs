using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SnakeEngine
{
    public class TimeSnakeGame : ClassicSnakeGame
    {
        private DispatcherTimer durationTimer;

        private TimerEventArgs timerEventArgs = new TimerEventArgs();

        public TimeSnakeGame(Canvas canvas, GameSettings gameSettings, Window window)
            : base(canvas, gameSettings, window)
        {
            Score = new TimeScore();
            TimeScore score = (TimeScore)Score;

            switch (((TimeGameSettings)gameSettings).Duration)
            {
                case GameDuration.OneMinute:
                    score.Time = new TimeSpan(0, 0, 1, 0);
                    break;
                case GameDuration.TwoMinutes:
                    score.Time = new TimeSpan(0, 0, 2, 0);
                    break;
                case GameDuration.ThreeMinutes:
                    score.Time = new TimeSpan(0, 0, 3, 0);
                    break;
            }

            durationTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            durationTimer.Tick += DurationTimer_Tick;
        }

        private void DurationTimer_Tick(object sender, EventArgs e)
        {
            TimeScore timeScore = (TimeScore)Score;

            //Subtruct one second from time
            if (!(timeScore.Time < new TimeSpan(0, 0, 0, 1)))
            {
                timeScore.Time = timeScore.Time.Subtract(new TimeSpan(0, 0, 0, 1));
                RaiseTickEvent();
            }
            else
            {
                durationTimer.Stop();
                timer.Stop();
                isAlive = false;
                RaiseSnakeIsDeadEvent();
            }
        }

        protected virtual void RaiseTickEvent()
        {
            if (Tick != null)
            {
                timerEventArgs.RestOfTime = ((TimeScore) Score).Time;
                Tick(this, timerEventArgs);
            }
        }

        public override void Start()
        {
            base.Start();
            durationTimer.Start();
        }

        protected override void RaiseSnakeIsDeadEvent()
        {
            base.RaiseSnakeIsDeadEvent();
            durationTimer.Stop();
        }

        public event EventHandler<TimerEventArgs> Tick;     
    }

}
