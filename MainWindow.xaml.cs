using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HT.Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in grid.Children.Cast<UIElement>())
            {
                if (item is Button button && "Number".Equals(button.Tag))
                {
                    button.Content = null;
                    button.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                }
            }
        }

        private void Scan_Click(object sender, RoutedEventArgs e)
        {

        }

        private int[,] Data { get; set; }

        private void Solve_Click(object sender, RoutedEventArgs e)
        {
            Data = GetData();
            var data = (int[,])Data.Clone();
            Sudoku.Solve(data);
            SetData(data);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var point = button.PointToScreen(new Point(0, 0));
            var b1 = Grid.GetRow(button) < 5;
            var b2 = Grid.GetColumn(button) < 5;
            if (b1 && b2)
            {
                point.X += 50;
                point.Y += 50;
            }
            else if (b1 && !b2)
            {
                point.X -= 150;
                point.Y += 50;
            }
            else if (!b1 && b2)
            {
                point.X += 50;
                point.Y -= 200;
            }
            else
            {
                point.X -= 150;
                point.Y -= 200;
            }
            var numberSelect = new NumberSelect(point);
            if (numberSelect.ShowDialog() == true)
            {
                button.Content = numberSelect.Number;
                button.Foreground = new SolidColorBrush(Colors.Black);
                if (!Sudoku.CheckValue(GetData(), Grid.GetRow(button), Grid.GetColumn(button)))
                {
                    button.Content = null;
                    button.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                }
            }
        }
    }

    public partial class MainWindow
    {
        private int[,] GetData()
        {
            var data = new int[9, 9];
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    foreach (var item in grid.Children.Cast<UIElement>().Where(ui => Grid.GetRow(ui) == i && Grid.GetColumn(ui) == j))
                    {
                        if (item is Button button)
                        {
                            data[i, j] = (button.Foreground as SolidColorBrush).Color == Colors.Black ? (int)button.Content : 0;
                        }
                    }
                }
            }
            return data;
        }

        private void SetData(int[,] data)
        {
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    foreach (var item in grid.Children.Cast<UIElement>().Where(ui => Grid.GetRow(ui) == i && Grid.GetColumn(ui) == j))
                    {
                        if (item is Button button)
                        {
                            button.Content = data[i, j];
                        }
                    }
                }
            }
        }
    }
}
