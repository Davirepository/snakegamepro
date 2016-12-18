using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SnakeDAL.Models;
using SnakeEngine;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ClassicSnakeGame snakeGame;
        private TimeGameSettings settings;

        public MainWindow()
        {
            InitializeComponent();

            settings = new TimeGameSettings()
            {
                SnakeBodyElemetSize = 20,
                FieldBorderThickness = 10,
                Speed = GameSpeed.Normal,
                GameMode = GameMode.Classic,
                Duration = GameDuration.None
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (settings.Speed)
            {
                case GameSpeed.Normal:
                    NormalRadioButton.IsChecked = true;
                    break;
                case GameSpeed.High:
                    HighRadioButton.IsChecked = true;
                    break;
                case GameSpeed.Extreme:
                    ExtremeRadioButton.IsChecked = true;
                    break;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (snakeGame == null) return;

                if (!snakeGame.IsAlive)
                {
                    if (e.Key != Key.Up && e.Key != Key.Left && e.Key != Key.Right && e.Key != Key.Down) return;

                    StartTipGrid.Visibility = Visibility.Hidden;
                    snakeGame.PressedKey = e.Key;
                    snakeGame.Start();
                }
                else
                {
                    snakeGame.PressedKey = e.Key;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
#endif
            }
        }

        private void WelcomeStartBtn_Click(object sender, RoutedEventArgs e)
        {
            WelcomeStackPanel.Visibility = Visibility.Hidden;
            GameModeStackPanel.Visibility = Visibility.Visible;
        }

        private void ClassicModeBtn_Click(object sender, RoutedEventArgs e)
        {
            GameGrid.Visibility = Visibility.Visible;
            MenuGrid.Visibility = Visibility.Hidden;
            Canvas.Visibility = Visibility.Visible;

            settings.GameMode = GameMode.Classic;
            settings.Duration = GameDuration.None;

            InitGame();
        }

        private void TimeModeBtn_Click(object sender, RoutedEventArgs e)
        {
            GameModeStackPanel.Visibility = Visibility.Hidden;
            TimeDurationStackPanel.Visibility = Visibility.Visible;
        }

        private void ExtraModeBtn_Click(object sender, RoutedEventArgs e)
        {
            GameGrid.Visibility = Visibility.Visible;
            MenuGrid.Visibility = Visibility.Hidden;
            Canvas.Visibility = Visibility.Visible;

            settings.GameMode = GameMode.Extra;
            settings.Duration = GameDuration.None;

            InitGame();
        }

        private void SnakeGame_SnakeGotPoint(object sender, EventArgs e)
        {
            ScoreTextBlock.Text = snakeGame.Score.Points.ToString();
        }

        private void SnakeGame_SnakeIsDead(object sender, EventArgs e)
        {
            GameOverGrid.Visibility = Visibility.Visible;
            GameOverScoreTextBlock.Text = snakeGame.Score.Points.ToString();

            var repository = new SnakeDAL.Repository.SnakeDbRepository();
            try
            {
                repository.AddResult(settings, snakeGame.Score, DateTime.Now);
                repository.Save();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
#endif
            }
            finally
            {
                repository.Dispose();
            }

            snakeGame = null;
        }

        private void PlayAgainBtn_Click(object sender, RoutedEventArgs e)
        {
            Canvas.Children.Clear();
            InitGame();

            GameOverGrid.Visibility = Visibility.Hidden;
            StartTipGrid.Visibility = Visibility.Visible;
        }

        private void TimeGameTick(object sender, TimerEventArgs e)
        {
            TimeTextBlock.Text = e.RestOfTime.ToString(@"mm\:ss");
        }

        private void BackToMenuFromGameBtn_Click(object sender, RoutedEventArgs e)
        {
            Canvas.Children.Clear();
            snakeGame = null;

            MenuGrid.Visibility = Visibility.Visible;
            GameModeStackPanel.Visibility = Visibility.Visible;
            GameGrid.Visibility = Visibility.Hidden;
            GameOverGrid.Visibility = Visibility.Hidden;
            StartTipGrid.Visibility = Visibility.Visible;
        }

        private void BackToMenuFromSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingsStackPanel.Visibility = Visibility.Hidden;
            WelcomeStackPanel.Visibility = Visibility.Visible;
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            WelcomeStackPanel.Visibility = Visibility.Hidden;
            SettingsStackPanel.Visibility = Visibility.Visible;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            string radioContent = ((RadioButton)sender).Content.ToString();

            switch (radioContent)
            {
                case "Normal":
                    settings.Speed = GameSpeed.Normal;
                    break;
                case "High":
                    settings.Speed = GameSpeed.High;
                    break;
                case "Extreme":
                    settings.Speed = GameSpeed.Extreme;
                    break;
            }
        }

        private void BackToMenuFromGameModeMenu_Click(object sender, RoutedEventArgs e)
        {
            GameModeStackPanel.Visibility = Visibility.Hidden;
            WelcomeStackPanel.Visibility = Visibility.Visible;
        }

        private void BackToMenuFromTipBtn_Click(object sender, RoutedEventArgs e)
        {
            GameGrid.Visibility = Visibility.Hidden;
            MenuGrid.Visibility = Visibility.Visible;
            GameModeStackPanel.Visibility = Visibility.Visible;
            Canvas.Children.Clear();
            snakeGame = null;
        }

        private void OneMinuteTimeModeBtn_Click(object sender, RoutedEventArgs e)
        {
            InitTimeMode(GameDuration.OneMinute);
        }

        private void TwoMinutesTimeModeBtn_Click(object sender, RoutedEventArgs e)
        {
            InitTimeMode(GameDuration.TwoMinutes);
        }

        private void ThreeMinutesTimeModeBtn_Click(object sender, RoutedEventArgs e)
        {
            InitTimeMode(GameDuration.ThreeMinutes);
        }

        private void BackToMenuFromTimeDuration_Click(object sender, RoutedEventArgs e)
        {
            TimeDurationStackPanel.Visibility = Visibility.Hidden;
            GameModeStackPanel.Visibility = Visibility.Visible;
        }

        private void ClassicModeHighScoresBtn_Click(object sender, RoutedEventArgs e)
        {
            HighScoresMainStackPanel.Visibility = Visibility.Hidden;
            HighScoresResultsGrid.Visibility = Visibility.Visible;
            HighScoresModeTitleTextBlock.Text = "classic mode";

            settings.GameMode = GameMode.Classic;

            InitHighScoreResults();
        }
        private void TimeModeHighScoresBtn_Click(object sender, RoutedEventArgs e)
        {
            HighScoresMainStackPanel.Visibility = Visibility.Hidden;
            TimeDurationHighScoreStackPanel.Visibility = Visibility.Visible;
            HighScoresModeTitleTextBlock.Text = "time mode";
        }

        private void ExtraModeHighScoresBtn_Click(object sender, RoutedEventArgs e)
        {
            HighScoresMainStackPanel.Visibility = Visibility.Hidden;
            HighScoresResultsGrid.Visibility = Visibility.Visible;
            HighScoresModeTitleTextBlock.Text = "extra mode";

            settings.GameMode = GameMode.Extra;

            InitHighScoreResults();
        }

        private void HighScoreBtn_Click(object sender, RoutedEventArgs e)
        {
            WelcomeStackPanel.Visibility = Visibility.Hidden;
            HighScoresMainStackPanel.Visibility = Visibility.Visible;
        }

        private void FillDataGrid(DataGrid dataGrid, IEnumerable<RankingResultModel> models)
        {
            dataGrid.Visibility = Visibility.Visible;

            dataGrid.Items.Clear();

            foreach (var model in models)
            {
                dataGrid.Items.Add(model);
            }
        }

        private void BackToMenuFromHighScoresResultsBtn_Click(object sender, RoutedEventArgs e)
        {
            HighScoresResultsGrid.Visibility = Visibility.Hidden;
            HighScoresMainStackPanel.Visibility = Visibility.Visible;
        }

        private void BackToMenuFromHighScoresBtn_Click(object sender, RoutedEventArgs e)
        {
            HighScoresMainStackPanel.Visibility = Visibility.Hidden;
            WelcomeStackPanel.Visibility = Visibility.Visible;
        }

        private void OneMinuteHighScoreTimeModeBtn_Click(object sender, RoutedEventArgs e)
        {
            SetHighScoreTimeDuration(GameDuration.OneMinute);
        }

        private void TwoMinutesHighScoreTimeModeBtn_Click(object sender, RoutedEventArgs e)
        {
            SetHighScoreTimeDuration(GameDuration.TwoMinutes);
        }

        private void ThreeMinutesHighScoreTimeModeBtn_Click(object sender, RoutedEventArgs e)
        {
            SetHighScoreTimeDuration(GameDuration.ThreeMinutes);
        }

        private void BackToMenuFromTimeDurationHighScore_Click(object sender, RoutedEventArgs e)
        {
            TimeDurationHighScoreStackPanel.Visibility = Visibility.Hidden;
            HighScoresMainStackPanel.Visibility = Visibility.Visible;
        }


        #region Helpers
        private void InitGame()
        {
            ScoreTextBlock.Text = "0";
            TimeTextBlock.Text = "";

            TimeBlockStackPanel.Visibility = Visibility.Hidden;

            switch (settings.GameMode)
            {
                case GameMode.Classic:
                    snakeGame = new ClassicSnakeGame(Canvas, settings, this);
                    break;
                case GameMode.Time:
                    snakeGame = new TimeSnakeGame(Canvas, settings, this);
                    ((TimeSnakeGame)snakeGame).Tick += TimeGameTick;
                    TimeBlockStackPanel.Visibility = Visibility.Visible;
                    break;
                case GameMode.Extra:
                    snakeGame = new ExtraSnakeGame(Canvas, settings, this);
                    break;
            }

            snakeGame.SnakeIsDead += SnakeGame_SnakeIsDead;
            snakeGame.SnakeGotPoint += SnakeGame_SnakeGotPoint;
        }

        private void InitTimeMode(GameDuration duration)
        {
            GameGrid.Visibility = Visibility.Visible;
            MenuGrid.Visibility = Visibility.Hidden;
            Canvas.Visibility = Visibility.Visible;
            GameModeStackPanel.Visibility = Visibility.Hidden;
            TimeDurationStackPanel.Visibility = Visibility.Hidden;

            settings.GameMode = GameMode.Time;

            settings.Duration = duration;

            InitGame();
        }
        private void InitHighScoreResults()
        {
            var repository = new SnakeDAL.Repository.SnakeDbRepository();

            try
            {
                NormalSpeedNoResultsTextBlock.Visibility = Visibility.Hidden;
                HighSpeedNoResultsTextBlock.Visibility = Visibility.Hidden;
                ExtremeSpeedNoResultsTextBlock.Visibility = Visibility.Hidden;


                //Getting data to model views

                var dbResults = new List<List<SnakeDAL.Context.RankingPlace>>();
                var modelResults = new List<List<RankingResultModel>>();

                dbResults.Add(repository.GetRankingInGameModeBySpeed(settings, GameSpeed.Normal).ToList());
                dbResults.Add(repository.GetRankingInGameModeBySpeed(settings, GameSpeed.High).ToList());
                dbResults.Add(repository.GetRankingInGameModeBySpeed(settings, GameSpeed.Extreme).ToList());


                List<RankingResultModel> currentModels;

                for (int i = 0; i < dbResults.Count; i++)
                {
                    currentModels = new List<RankingResultModel>();

                    for (int j = 0; j < dbResults[i].Count; j++)
                    {
                        currentModels.Add(
                            new RankingResultModel()
                            {
                                Place = (j + 1).ToString() + ".",
                                Score = dbResults[i][j].Points.ToString(),
                                Date = string.Format("{0}:{1}", dbResults[i][j].TimeOfResult.ToShortDateString(),
                                    dbResults[i][j].TimeOfResult.ToShortTimeString())
                            });
                    }
                    modelResults.Add(currentModels);
                }

                //Setting UI Lists of results

                if (modelResults[0].Count == 0)
                {
                    NormalSpeedNoResultsTextBlock.Visibility = Visibility.Visible;
                    NormalSpeedResultsDataGrid.Visibility = Visibility.Hidden;
                }
                if (modelResults[1].Count == 0)
                {
                    HighSpeedNoResultsTextBlock.Visibility = Visibility.Visible;
                    HighSpeedResultsDataGrid.Visibility = Visibility.Hidden;
                }
                if (modelResults[2].Count == 0)
                {
                    ExtremeSpeedNoResultsTextBlock.Visibility = Visibility.Visible;
                    ExtremeSpeedResultsDataGrid.Visibility = Visibility.Hidden;
                }

                FillDataGrid(NormalSpeedResultsDataGrid, modelResults[0]);
                FillDataGrid(HighSpeedResultsDataGrid, modelResults[1]);
                FillDataGrid(ExtremeSpeedResultsDataGrid, modelResults[2]);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
#endif
            }
            finally
            {
                repository.Dispose();
            }
        }
        private void SetHighScoreTimeDuration(GameDuration duration)
        {
            TimeDurationHighScoreStackPanel.Visibility = Visibility.Hidden;
            HighScoresResultsGrid.Visibility = Visibility.Visible;

            settings.GameMode = GameMode.Time;
            settings.Duration = duration;

            InitHighScoreResults();
        }

        #endregion

    }
}
