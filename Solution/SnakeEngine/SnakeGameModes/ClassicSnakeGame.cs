using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeEngine
{
    public class ClassicSnakeGame
    {   
        protected Canvas canvas;

        protected DispatcherTimer timer;

        protected GameSettings settings;

        protected Snake snake;

        protected Random random = new Random();

        protected int fieldPartitionMaxXSize;

        protected int fieldPartitionMaxYSize;

        protected int fieldPartitionXrest;
        protected int fieldPartitionYrest;

        protected Window window;

        protected BaseElement currentFoodElement;

        protected Key pressedKey;

        protected bool isAlive;

        public bool IsAlive
        {
            get { return isAlive; }
        }

        public Score Score { get; protected set; }

        public Key PressedKey
        {
            get { return pressedKey; }
            set
            {
                if (value == Key.Left || value == Key.Up
                    || value == Key.Right || value == Key.Down)
                    pressedKey = value;
            }
        }

        //Constructor
        public ClassicSnakeGame(Canvas canvas, GameSettings gameSettings, Window window)
        {
            this.canvas = canvas;
            settings = gameSettings;
            Score = new Score();
            
            this.window = window;

            fieldPartitionMaxXSize = ((int)canvas.ActualWidth - settings.FieldBorderThickness * 2) / gameSettings.SnakeBodyElemetSize;
            fieldPartitionMaxYSize = ((int)canvas.ActualHeight - settings.FieldBorderThickness * 2) / gameSettings.SnakeBodyElemetSize;

            fieldPartitionXrest = ((int)canvas.ActualWidth - settings.FieldBorderThickness * 2) % fieldPartitionMaxXSize;
            fieldPartitionYrest = ((int)canvas.ActualHeight - settings.FieldBorderThickness * 2) % fieldPartitionMaxYSize;

            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 1000/ (int) settings.Speed)
            };

            timer.Tick += Refresh;

            InitializeDrawElemets();
        }


        protected void InitializeDrawElemets()
        {
            //Creating snake and setting it's head to screen center
            snake = new Snake((fieldPartitionMaxXSize / 2) * settings.SnakeBodyElemetSize + settings.FieldBorderThickness,
                (fieldPartitionMaxYSize / 2) * settings.SnakeBodyElemetSize + settings.FieldBorderThickness, settings.SnakeBodyElemetSize);

            Canvas.SetTop(snake.Body[0].DrawElement, snake.Body[0].Y);
            Canvas.SetLeft(snake.Body[0].DrawElement, snake.Body[0].X);
            canvas.Children.Add(snake.Body[0].DrawElement);

            DrawBorders();
        }

        protected void DrawBorders()
        {
            Rectangle border = new Rectangle();
            border.StrokeThickness = settings.FieldBorderThickness;
            border.Stroke = new SolidColorBrush(Color.FromArgb(200, 100, 20, 123));
            border.Fill = canvas.Background;
            border.Width = canvas.ActualWidth;
            border.Height = canvas.ActualHeight;
            Canvas.SetZIndex(border, -1);
            window.Dispatcher.Invoke(() =>
                {
                    Canvas.SetTop(border, 0);
                    Canvas.SetLeft(border, 0);
                    canvas.Children.Add(border);
                }
            );

            GenerateFood();
        }

        //Random food generation
        protected virtual void GenerateFood()
        {
            var food = new FoodElement(settings.FieldBorderThickness + random.Next(0, (int)fieldPartitionMaxXSize) * settings.SnakeBodyElemetSize,
                settings.FieldBorderThickness + random.Next(0, (int)fieldPartitionMaxYSize) * settings.SnakeBodyElemetSize, settings.SnakeBodyElemetSize);

            currentFoodElement = food;

            window.Dispatcher.Invoke(() =>
            {
                Canvas.SetTop(food.DrawElement, food.Y);
                Canvas.SetLeft(food.DrawElement, food.X);
                canvas.Children.Add(food.DrawElement);
            });
        }


        protected void RefreshSnakeDirection()
        {
            switch (PressedKey)
            {
                case Key.Left:
                    if (snake.Direction != SnakeDirection.Right)
                        snake.Direction = SnakeDirection.Left;
                    break;

                case Key.Up:
                    if (snake.Direction != SnakeDirection.Down)
                        snake.Direction = SnakeDirection.Up;
                    break;

                case Key.Right:
                    if (snake.Direction != SnakeDirection.Left)
                        snake.Direction = SnakeDirection.Right;
                    break;
                case Key.Down:
                    if (snake.Direction != SnakeDirection.Up)
                        snake.Direction = SnakeDirection.Down;
                    break;
            }
        }

        protected void Refresh(object sender, EventArgs e)
        {
            RefreshSnakeDirection();

            for (int i = snake.Body.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    //Move the head
                    switch (snake.Direction)
                    {
                        case SnakeDirection.Up:
                            snake.Body[i].Y -= snake.BodyElementSize;
                            break;
                        case SnakeDirection.Left:
                            snake.Body[i].X -= snake.BodyElementSize;
                            break;
                        case SnakeDirection.Right:
                            snake.Body[i].X += snake.BodyElementSize;
                            break;
                        case SnakeDirection.Down:
                            snake.Body[i].Y += snake.BodyElementSize;
                            break;
                    }

                    //Calculating distance between food element and the head of snake
                    int distance = (int)Math.Sqrt(Math.Pow(currentFoodElement.X - snake.Body[i].X, 2)
                        + Math.Pow(currentFoodElement.Y - snake.Body[i].Y, 2));

                    //if the head collided with the field's border
                    if (snake.Body[i].X - settings.FieldBorderThickness < 0 ||
                        snake.Body[i].Y - settings.FieldBorderThickness < 0 ||
                        snake.Body[i].X + settings.SnakeBodyElemetSize + settings.FieldBorderThickness> canvas.ActualWidth ||
                        snake.Body[i].Y + settings.SnakeBodyElemetSize + settings.FieldBorderThickness> canvas.ActualHeight)
                    {
                        RaiseSnakeIsDeadEvent();
                        return;
                    }


                    for (int j = snake.Body.Count-1; j > 0; j--)
                    {
                        if (snake.Body[0].X == snake.Body[j].X && 
                            snake.Body[0].Y == snake.Body[j].Y)
                        {
                            RaiseSnakeIsDeadEvent();
                            return;
                        }
                    }

                    ExcecuteAdditionalFeature();

                    //if head is too close to food
                    if (distance <= settings.SnakeBodyElemetSize / 2)
                    {
                        //delete food element from canvas
                        canvas.Children.Remove(currentFoodElement.DrawElement);
                        snake.Raise();
                        canvas.Children.Add(snake.Body[snake.Body.Count - 1].DrawElement);
                        GenerateFood();
                        Score.Points++;
                        RaiseSnakeGotPointEvent();
                    }

                }
                else
                {
                    //Move the rest of body
                    snake.Body[i].X = snake.Body[i - 1].X;
                    snake.Body[i].Y = snake.Body[i - 1].Y;
                }
            }
            window.Dispatcher.Invoke(Render);
        }

        protected virtual void RaiseSnakeGotPointEvent()
        {
            if(SnakeGotPoint != null)
                SnakeGotPoint(this, EventArgs.Empty);
        }

        protected virtual void RaiseSnakeIsDeadEvent()
        {
            isAlive = false;
            timer.Stop();

            if (SnakeIsDead != null)
                SnakeIsDead(this, EventArgs.Empty);
        }

        protected virtual void ExcecuteAdditionalFeature()
        {                                   
        }

        protected void Render()
        {
            if (!isAlive) return;
            foreach (SankeBodyElement element in snake.Body)
            {
                Canvas.SetLeft(element.DrawElement, element.X);
                Canvas.SetTop(element.DrawElement, element.Y);
            }
        }

        public virtual void Start()
        {
            timer.Start();
            isAlive = true;
        }

        public event EventHandler<EventArgs> SnakeGotPoint;
        public event EventHandler<EventArgs> SnakeIsDead;
    }
}
