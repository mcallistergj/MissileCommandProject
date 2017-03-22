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
using System.Windows.Threading;

namespace mcallistergcscd371missilecommand
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    DispatcherTimer enemyLaunchTimer;
    DispatcherTimer missileTimer;
    DispatcherTimer defenderTimer;
    internal List<Missile> enemyMissiles = new List<Missile>();
    List<Missile> defendingMissiles = new List<Missile>();
    private int number_of_defense_missiles;
    private int enemy_missile_to_launch;
    internal int enemy_missiles_live;
    private int enemy_missile_speed = 101;
    private int missile_launch_frequency = 2;

    public MainWindow()
    {
      InitializeComponent();
      start_game();
    }

    public void start_game()
    {
      number_of_defense_missiles = 30;
      enemy_missile_to_launch = 3;
      enemy_missiles_live = 0;
      missileTimer = new DispatcherTimer();
      defenderTimer = new DispatcherTimer();
      defenderTimer.Tick += new EventHandler(defenderTimer_Tick);
      defenderTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
      missileTimer.Tick += new EventHandler(missileTimer_Tick);
      missileTimer.Interval = new TimeSpan(0, 0, 0, 0, enemy_missile_speed);
      enemyLaunchTimer = new DispatcherTimer();
      enemyLaunchTimer.Tick += new EventHandler(enemyLaunch_Tick);
      enemyLaunchTimer.Interval = new TimeSpan(0, 0, 0, missile_launch_frequency);
      enemyLaunchTimer.Start();
    }

    private void next_level()
    {
      if (enemy_missile_speed > 50)
      {
        enemy_missile_speed -= 50;
      }
      missileTimer.Stop();
      enemyLaunchTimer.Stop();
      defenderTimer.Stop();
      start_game();
    }

    private void enemyLaunch_Tick(object sender, EventArgs e)
    {
      if (enemy_missile_to_launch > 0)
      {
        launchEnemyMissile(sender, e);
        enemy_missile_to_launch--;
        enemy_missiles_live++;
      }
      else if(enemy_missiles_live == 0)
      {
        next_level();
      }
    }

    private void defenderTimer_Tick(object sender, EventArgs e)
    {
      double radianAngle;
      foreach (Missile missile in defendingMissiles)
      {
        if (!missile.Exploded)
        {
          if (missile.Target.X > missile.X1)
          {
            radianAngle = Math.Atan2((missile.Y1 - missile.Target.Y), (missile.Target.X - missile.X1));
            missile.FireAngle = 90.0 / (radianAngle * (180 / Math.PI));
            missile.X2 += missile.FireAngle - 1;
          }
          else if (missile.Target.X < missile.X1)
          {
            radianAngle = Math.Atan2((missile.Y1 - missile.Target.Y), (missile.X1 - missile.Target.X));
            missile.FireAngle = 90.0 / (radianAngle * (180 / Math.PI));
            missile.X2 -= missile.FireAngle - 1;
          }
          missile.Y2 -= 1;
          if (missile.Y2 == 0 ||
            missile.X2 > backgroundCanvas.ActualWidth ||
            missile.X2 < 0)
          {
            backgroundCanvas.Children.Remove(missile.MissileLine);
          }
          if(missile.Y2 == missile.Target.Y || 
            (missile.X1 < missile.Target.X && missile.X2 > missile.Target.X)||
            (missile.X1 > missile.Target.X && missile.X2 < missile.Target.X))
          {
            missile.detonate(this);
            backgroundCanvas.Children.Remove(missile.MissileLine);
          }
        }
      }
    }

    private void missileTimer_Tick(object sender, EventArgs e)
    {
      foreach (Missile missile in enemyMissiles)
      {
        if (missile.X2 < missile.X1)
        {
          missile.X2 -= 3*(.3);
        }
        else if(missile.X2 > missile.X1)
        {
          missile.X2 += 3*(.3);
        }
        missile.Y2 += (3);
        if (missile.Y2 == backgroundCanvas.ActualHeight ||
          missile.X2 > backgroundCanvas.ActualWidth || 
          missile.X2 < 0)
        {
          backgroundCanvas.Children.Remove(missile.MissileLine);
          if (!missile.Exploded)
          {
            missile.Exploded = true;
            enemy_missiles_live--;
          }
        }
      }
    }


    private void launchEnemyMissile(object sender, EventArgs e)
    {
      Missile missile = new Missile();
      Random rand = new Random();
      missile.X1 = rand.Next(0, 615);
      missile.Y1 = -10;
      missile.X2 = rand.Next((int)missile.X1 - 2, (int)missile.X1 + 3);
      missile.Y2 = 0;
      enemyMissiles.Add(missile);
      backgroundCanvas.Children.Add(missile.MissileLine);
      missileTimer.Start();
    }

    private void fireDefenseMissile(object sender, MouseButtonEventArgs e)
    {
      if (number_of_defense_missiles > 0)
      {
        Missile defenseMissile = new Missile();
        Point target = e.GetPosition(this);
        defenseMissile.X1 = backgroundCanvas.ActualWidth / 2;
        defenseMissile.Y1 = backgroundCanvas.ActualHeight;
        defenseMissile.X2 = backgroundCanvas.ActualWidth / 2;
        defenseMissile.Y2 = backgroundCanvas.ActualHeight;
        defenseMissile.Target = target;
        backgroundCanvas.Children.Add(defenseMissile.MissileLine);
        defendingMissiles.Add(defenseMissile);
        number_of_defense_missiles--;
        defenderTimer.Start();
      }
    }

  }


}



/*
public MainWindow()
{
  InitializeComponent();

  dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
  dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
  dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);//days, hours, mins, secs, millis

}

private void dispatcherTimer_Tick(object sender, EventArgs e)
{
  myLine.X2 = myLine.X2 + 2;
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

  */