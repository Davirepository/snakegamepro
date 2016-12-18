using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeEngine
{
    public class ExtraSnakeGame : ClassicSnakeGame
    {
        private ICollection<Shape> Walls = new List<Shape>();

        public ExtraSnakeGame(Canvas canvas, GameSettings gameSettings, Window window)
            : base(canvas, gameSettings, window)
        {
            int snakeXPos = settings.SnakeBodyElemetSize * 4 + settings.FieldBorderThickness;
            int snakeYPos = settings.SnakeBodyElemetSize * 10 + settings.FieldBorderThickness;

            snake.Body[0].X = snakeXPos;
            snake.Body[0].Y = snakeYPos;

            Canvas.SetTop(snake.Body[0].DrawElement, snakeYPos);
            Canvas.SetLeft(snake.Body[0].DrawElement, snakeXPos);
        }


        private void DrawWalls()
        {
            int a = 20;
            int b = 3;

            Rectangle rectangle1 = new Rectangle();
            rectangle1.Fill = new SolidColorBrush(Color.FromRgb(50, 50, 50));
            rectangle1.Width = settings.SnakeBodyElemetSize * a;
            rectangle1.Height = settings.SnakeBodyElemetSize * b;
            int xPos = (fieldPartitionMaxXSize - a) / 2 * settings.SnakeBodyElemetSize + settings.FieldBorderThickness;
            int yPos = (fieldPartitionMaxYSize - b) / 2 * settings.SnakeBodyElemetSize + settings.FieldBorderThickness;

            Canvas.SetLeft(rectangle1, xPos);
            Canvas.SetTop(rectangle1, yPos);
            canvas.Children.Add(rectangle1);


            a = 15;
            Rectangle rectangle2 = new Rectangle();
            rectangle2.Fill = new SolidColorBrush(Color.FromRgb(50, 50, 50));
            rectangle2.Width = settings.SnakeBodyElemetSize * b;
            rectangle2.Height = settings.SnakeBodyElemetSize * a;
            xPos = (fieldPartitionMaxXSize - b) / 2 * settings.SnakeBodyElemetSize + settings.FieldBorderThickness;
            yPos = (fieldPartitionMaxYSize - a) / 2 * settings.SnakeBodyElemetSize + settings.FieldBorderThickness;

            Canvas.SetLeft(rectangle2, xPos);
            Canvas.SetTop(rectangle2, yPos);
            canvas.Children.Add(rectangle2);

            Walls.Add(rectangle1);
            Walls.Add(rectangle2);
        }

        //Checking collision with walls
        protected override void ExcecuteAdditionalFeature()
        {
            foreach (var wall in Walls)
            {
                int left = (int)Canvas.GetLeft(wall);
                int top = (int)Canvas.GetTop(wall);

                if (snake.Body[0].X >= left && snake.Body[0].X < left + wall.Width &&
                    snake.Body[0].Y >= top && snake.Body[0].Y < top + wall.Height)
                {
                    RaiseSnakeIsDeadEvent();
                    break;
                }
            }
        }
        
        
        protected override void GenerateFood()
        {
            DrawWalls();

            int leftFoodPos, topFoodPos;
            bool isFoodPositionCorrect = false;

            //Generation food while it's random position falls into wall
            do
            {
                leftFoodPos = settings.FieldBorderThickness + random.Next(0, (int)fieldPartitionMaxXSize) * settings.SnakeBodyElemetSize;
                topFoodPos = settings.FieldBorderThickness + random.Next(0, (int)fieldPartitionMaxYSize) * settings.SnakeBodyElemetSize;

                foreach (var wall in Walls)
                {
                    int left = (int)Canvas.GetLeft(wall);
                    int top = (int)Canvas.GetTop(wall);

                    if (!(leftFoodPos >= left && leftFoodPos < left + wall.Width &&
                        topFoodPos >= top && topFoodPos < top + wall.Height))
                    {
                        isFoodPositionCorrect = true;
                    }
                    else
                    {
                        isFoodPositionCorrect = false;
                        break;
                    }
                }

            } while (!isFoodPositionCorrect);

            //Applying food's position and create it when it outside the walls
            var food = new FoodElement(leftFoodPos, topFoodPos, settings.SnakeBodyElemetSize);

            currentFoodElement = food;

            window.Dispatcher.Invoke(() =>
            {
                Canvas.SetTop(food.DrawElement, food.Y);
                Canvas.SetLeft(food.DrawElement, food.X);
                canvas.Children.Add(food.DrawElement);
            });
        }
    }
}
