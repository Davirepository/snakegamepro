namespace SnakeEngine
{
    public enum GameSpeed
    {
        Normal = 10,
        High = 15,
        Extreme = 25
    }

    public enum GameDuration
    {
        OneMinute = 1,
        TwoMinutes = 2,
        ThreeMinutes = 3,
        None = 4
    }


    public enum GameMode
    {
        Classic,
        Time,
        Extra
    }

    public class GameSettings
    {
        public int SnakeBodyElemetSize { get; set; }

        public GameSpeed Speed { get; set; }
        public GameMode GameMode { get; set; }

        public int FieldBorderThickness { get; set; }
    }

    public class TimeGameSettings : GameSettings
    {
        public GameDuration Duration { get; set; }
    }
}
