/*using System;
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


namespace WPFDrawingTutorial
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Line myLine;
        private System.Windows.Threading.DispatcherTimer dispatcherTimer;


        public MainWindow()
        {
            InitializeComponent();
            
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);//days, hours, mins, secs, millis
            
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            myLine.X2 = myLine.X2+2;
        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            myLine = new Line();
            myLine.X1 = 10;
            myLine.Y1 = 10;
            myLine.X2 = 50;
            myLine.Y2 = 50;

            myLine.Stroke = Brushes.Black;// new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
            drawingCanvas.Children.Add(myLine);
            //MessageBox.Show(drawingCanvas.Children.Count.ToString());
            foreach (object o in drawingCanvas.Children)
                MessageBox.Show(o.ToString());
            Canvas c = (Canvas)drawingCanvas.Children[0];
            c.Background = Brushes.Brown; 

            
        }

        

        private void moveLineButton_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Start();
            
        }

        private void greenCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            MessageBox.Show(e.ToString());
        }

        private void drawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("mouse click on canvas");
        }

        private void drawingCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            MessageBox.Show("leaving canvas");
        }

    }
}*/
