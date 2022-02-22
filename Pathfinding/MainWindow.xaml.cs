using Pathfinding.Logic;
using Pathfinding.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Pathfinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int cols = 120;
        private int rows = 60;
        private double a;
        private double b;
        private (PathNode?, PathNode?) selectedPoints = (null, null);
        private Pathfinder pathfinder;
        private Rectangle[,] rectangles;
        private Brush[] brushes = new Brush[]
        {
            Brushes.AliceBlue,
            Brushes.Blue,
            Brushes.Cyan,
            Brushes.Green,
            Brushes.Magenta,
            Brushes.Orchid,
            Brushes.Red,
            Brushes.SlateBlue,
            Brushes.SteelBlue,
            Brushes.Yellow,
            Brushes.White,
            Brushes.Lime,
            Brushes.LimeGreen
        };

        public MainWindow()
        {
            InitializeComponent();

            pathfinder = new Pathfinder(rows, cols);
            rectangles = new Rectangle[rows, cols];
        }

        private void mainCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            a = mainCanvas.ActualWidth / cols;
            b = mainCanvas.ActualHeight / rows;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var rect = new Rectangle();
                    rect.Width = a;
                    rect.Height = b;
                    Canvas.SetLeft(rect, a * j);
                    Canvas.SetTop(rect, b * i);
                    rectangles[i, j] = rect;
                    mainCanvas.Children.Add(rect);
                }
            }
        }

        private void mainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = Mouse.GetPosition(mainCanvas);

            var rectColumn = (int)(mousePos.X / a);
            var rectRow = (int)(mousePos.Y / b);

            var clickedRectangle = rectangles[rectRow, rectColumn];
            var pathNode = new PathNode()
            {
                X = rectRow,
                Y = rectColumn
            };

            if (selectedPoints.Item1 is null)
            {
                selectedPoints.Item1 = pathNode;
                clickedRectangle.Fill = new SolidColorBrush(Colors.Green);
            } 
            else if (selectedPoints.Item2 is null)
            {
                selectedPoints.Item2 = pathNode;
                clickedRectangle.Fill = new SolidColorBrush(Colors.Green);

                try
                {
                    var path = pathfinder.GetPath(selectedPoints.Item1, selectedPoints.Item2);
                    var rnd = new Random();
                    var brush = brushes[rnd.Next(brushes.Length)];
                    foreach (var node in path)
                    {
                        var rect = rectangles[node.X, node.Y];
                        rect.Fill = brush;
                    }
                }
                catch (Exception ex)
                {
                    var rectStart = rectangles[selectedPoints.Item1.X, selectedPoints.Item1.Y];
                    rectStart.Fill = new SolidColorBrush(Colors.Transparent);
                    var rectEnd = rectangles[selectedPoints.Item2.X, selectedPoints.Item2.Y];
                    rectEnd.Fill = new SolidColorBrush(Colors.Transparent);

                    selectedPoints = (null, null);
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                selectedPoints = (pathNode, null);
                clickedRectangle.Fill = new SolidColorBrush(Colors.Green);
            }
        }
    }
}
