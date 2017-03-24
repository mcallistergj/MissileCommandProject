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
    internal int number_of_defense_missiles = 30;
    private int enemy_missile_to_launch;
    internal int enemy_missiles_live;
    private int enemy_missile_speed = 101;
    private int missile_launch_frequency = 2;
    internal int cities_to_defend = 6;
    internal bool increasing_missile_speed = false;
    internal int level = 1;
    internal int score = 0;

    public MainWindow()
    {
      InitializeComponent();
    }

    public void start_game()
    {
      enemy_missile_to_launch = 20;
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
      if (increasing_missile_speed && enemy_missile_speed > 10)
      {
        enemy_missile_speed -= 10;
      }
      missileTimer.Stop();
      enemyLaunchTimer.Stop();
      defenderTimer.Stop();
      level++;
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
      missile.Y1 = 15;
      missile.X2 = rand.Next((int)missile.X1 - 2, (int)missile.X1 + 3);
      missile.Y2 = 30;
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
        defenseMissile.Y1 = backgroundCanvas.ActualHeight - 20;
        defenseMissile.X2 = backgroundCanvas.ActualWidth / 2;
        defenseMissile.Y2 = backgroundCanvas.ActualHeight - 20;
        defenseMissile.Target = target;
        backgroundCanvas.Children.Add(defenseMissile.MissileLine);
        defendingMissiles.Add(defenseMissile);
        number_of_defense_missiles--;
        defenderTimer.Start();
      }
    }

    private void endGame(object sender, RoutedEventArgs e)
    {

    }

    private void setUpNewGame(object sender, EventArgs e)
    {
      OptionsWindow optionsWindow = new OptionsWindow(this);
      optionsWindow.ShowDialog();
    }

    private void exitGame(object sender, EventArgs e)
    {

    }

    private void openAboutWindow(object sender, EventArgs e)
    {

    }

    private void checkForDestruction(Missile missile)
    {
      if(missile.X2 > 21 && missile.X2 < 98 && missile.Y2 > backgroundCanvas.ActualHeight - 40)
      {
        cities_to_defend--;
        if(cities_to_defend < 4)
        {
          //city1Image.Visibility = false;
        }
      }
      else if (missile.X2 > 140 && missile.X2 < 223 && missile.Y2 > backgroundCanvas.ActualHeight - 40)
      {

      }
      else if (missile.X2 > 263 && missile.X2 < 349 && missile.Y2 > backgroundCanvas.ActualHeight - 40)
      {

      }
      else if (missile.X2 > 401 && missile.X2 < 484 && missile.Y2 > backgroundCanvas.ActualHeight - 40)
      {

      }
      else if (missile.X2 > 505 && missile.X2 < 591 && missile.Y2 > backgroundCanvas.ActualHeight - 40)
      {

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