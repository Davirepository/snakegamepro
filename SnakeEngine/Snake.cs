using System.Collections.Generic;
using System.Windows.Media;

namespace SnakeEngine
{
    public enum SnakeDirection
    {
        Left = 1,
        Up = 2,
        Right = 3,
        Down = 4
    }

    public class Snake
    {
        public SnakeDirection Direction { get; set; }

        //List represents full snake's body
        public List<SankeBodyElement> Body { get; }

        public int BodyElementSize { get; }

        public Snake(int x, int y, int bodyElementSize)
        {
            BodyElementSize = bodyElementSize;

            //Addind head to snake's body
            SankeBodyElement head = new SankeBodyElement(x, y, bodyElementSize);
            head.DrawElement.Fill = new SolidColorBrush(Color.FromRgb(15, 2, 180));
            Body = new List<SankeBodyElement>
            {
                head
            };
        }

        //Raise the snake
        public void Raise()
        {
            Body.Add(new SankeBodyElement(Body[Body.Count -1].X, Body[Body.Count -1].Y, BodyElementSize));
        }

    }
}
