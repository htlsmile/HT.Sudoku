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
    public partial class NumberSelect : Window
    {
        public NumberSelect(Point point)
        {
            InitializeComponent();
            Left = point.X;
            Top = point.Y;
        }

        private int number;

        public int Number { get => number; }

        private void Button_Click(object sender, RoutedEventArgs e) => DialogResult = int.TryParse((sender as Button).Content.ToString(), out number);
        
        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D1:
                case Key.NumPad1:
                    number = 1; DialogResult = true; break;
                case Key.D2:
                case Key.NumPad2:
                    number = 2; DialogResult = true; break;
                case Key.D3:
                case Key.NumPad3:
                    number = 3; DialogResult = true; break;
                case Key.D4:
                case Key.NumPad4:
                    number = 4; DialogResult = true; break;
                case Key.D5:
                case Key.NumPad5:
                    number = 5; DialogResult = true; break;
                case Key.D6:
                case Key.NumPad6:
                    number = 6; DialogResult = true; break;
                case Key.D7:
                case Key.NumPad7:
                    number = 7; DialogResult = true; break;
                case Key.D8:
                case Key.NumPad8:
                    number = 8; DialogResult = true; break;
                case Key.D9:
                case Key.NumPad9:
                    number = 9; DialogResult = true; break;
                default:
                    break;
            }
        }
    }
}
