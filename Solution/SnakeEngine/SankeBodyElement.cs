using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeEngine
{
    public class BaseElement
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Ellipse DrawElement { get; }

        //Size of snake's body element
        public int Size { get; }

        public BaseElement(int x, int y, int size)
        {
            Size = size;
            X = x;
            Y = y;

            DrawElement = new Ellipse();
            DrawElement.Height = Size;
            DrawElement.Width = Size;
        }
    }

    public class SankeBodyElement : BaseElement
    {
        public SankeBodyElement(int x, int y, int size)
            :base(x,y,size)
        {
            //Creating color of element
            SolidColorBrush color = new SolidColorBrush()
            {
                Color = Color.FromRgb(50, 50, 250)
            };

            DrawElement.Fill = color;
        }
    }


    public class FoodElement : BaseElement
    {
        public FoodElement(int x, int y, int size)
            :base(x,y,size)
        {
            SolidColorBrush color = new SolidColorBrush()
            {
                Color = Color.FromRgb(20,200,20)
            };

            DrawElement.Fill = color;
        }
    }


}
