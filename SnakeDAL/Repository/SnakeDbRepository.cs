using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeDAL.Context;
using SnakeEngine;
using GameMode = SnakeEngine.GameMode;

namespace SnakeDAL.Repository
{
    public class SnakeDbRepository : IDisposable
    {
        private SnakeContext context = new SnakeContext();

        public void AddResult(TimeGameSettings settings, Score score, DateTime timeOfResult)
        {
            string gameModeName = GetGameModeName(settings.GameMode);
            string gameSpeedName = GetSpeedName(settings.Speed);
            string gameTimeDurationName = GetTimeDurationName(settings.Duration);

            SnakeDAL.Context.GameMode gameMode = context.GameModes
                .Select(gm => gm).FirstOrDefault(gm => gm.GameModeName == gameModeName);

            SnakeDAL.Context.Speed gameSpeed = context.Speeds
                .Select(gs => gs).FirstOrDefault(gs => gs.SpeedName == gameSpeedName);

            SnakeDAL.Context.TimeDuration gameDuration = context.TimeDurations
                .Select(td => td).FirstOrDefault(td => td.TimeDurationName == gameTimeDurationName);

            RankingPlace rankingPlace = new RankingPlace()
            {
                Points = score.Points,
                TimeOfResult = timeOfResult,
                GameMode = gameMode,
                Speed = gameSpeed,
                TimeDuration = gameDuration
            };

            context.RankingPlaces.Add(rankingPlace);
        }

        public IEnumerable<RankingPlace> GetRankingInGameModeBySpeed(SnakeEngine.TimeGameSettings settings, SnakeEngine.GameSpeed gameSpeed)
        {
            string speedName = GetSpeedName(gameSpeed);

            if (settings.Duration == GameDuration.None)
                return GetRankingByGameMode(settings.GameMode)
                    .Where(rp => rp.Speed.SpeedName == speedName).OrderByDescending(rp => rp.Points).Take(10);

            return GetRankingInTimeModeByDuration(settings.Duration)
                .Where(rp => rp.Speed.SpeedName == speedName).OrderByDescending(rp => rp.Points).Take(10);
        }
        private IEnumerable<RankingPlace> GetRankingByGameMode(SnakeEngine.GameMode gameMode)
        {
            string gameModeName = GetGameModeName(gameMode);

            return context.RankingPlaces.Select(rp => rp).AsEnumerable()
                .Where(rp => rp.GameMode.GameModeName == gameModeName);
        }

        private IEnumerable<RankingPlace> GetRankingInTimeModeByDuration(SnakeEngine.GameDuration duration)
        {
            string timeDurationName = GetTimeDurationName(duration);
            return GetRankingByGameMode(GameMode.Time)
                .Where(rp => rp.TimeDuration.TimeDurationName == timeDurationName);
        }

        private string GetGameModeName(SnakeEngine.GameMode gameMode)
        {
            string result;
            switch (gameMode)
            {
                case SnakeEngine.GameMode.Classic:
                    result = "Classic";
                    break;
                case SnakeEngine.GameMode.Time:
                    result = "Time";
                    break;
                case SnakeEngine.GameMode.Extra:
                    result = "Extra";
                    break;
                default:
                    result = "Classic";
                    break;
            }
            return result;
        }

        private string GetSpeedName(SnakeEngine.GameSpeed gameSpeed)
        {
            string result;
            switch (gameSpeed)
            {
                case SnakeEngine.GameSpeed.Normal:
                    result = "Normal";
                    break;
                case SnakeEngine.GameSpeed.High:
                    result = "High";
                    break;
                case SnakeEngine.GameSpeed.Extreme:
                    result = "Extreme";
                    break;
                default:
                    result = "Normal";
                    break;
            }
            return result;
        }

        private string GetTimeDurationName(SnakeEngine.GameDuration gameDuration)
        {
            string result;
            switch (gameDuration)
            {
                case SnakeEngine.GameDuration.OneMinute:
                    result = "One minute";
                    break;
                case SnakeEngine.GameDuration.TwoMinutes:
                    result = "Two minutes";
                    break;
                case SnakeEngine.GameDuration.ThreeMinutes:
                    result = "Three minutes";
                    break;
                case SnakeEngine.GameDuration.None:
                    result = "None";
                    break;
                default:
                    result = "None";
                    break;
            }
            return result;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
