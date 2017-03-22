using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace mcallistergcscd371missilecommand
{
  class Explosion
  {
    private DispatcherTimer explosionTimer;
    private Point explosionCenter;
    private double explosionDiameter;
    private const double maxExplosionDiameter = 60;
    private List<Ellipse> explosionEllipses = new List<Ellipse>();
    private MainWindow mainWindow;
    private Missile missile;

    public Explosion(MainWindow main, Point center, Missile missile)
    {
      mainWindow = main;
      explosionTimer = new DispatcherTimer();
      explosionTimer.Tick += new EventHandler(explosionTimer_Tick);
      explosionTimer.Interval = new TimeSpan(0, 0, 0, 0, 25);
      explosionCenter = center;
      explosionDiameter = 1;
      this.missile = missile;
      explosionTimer.Start();
    }

    private void explosionTimer_Tick(object sender, EventArgs e)
    {
      if(explosionDiameter <= maxExplosionDiameter)
      {
        double left = explosionCenter.X - explosionDiameter / 2.0;
        double top = explosionCenter.Y - explosionDiameter / 2.0;
        double right = explosionCenter.X + explosionDiameter / 2.0;
        double bottom = explosionCenter.Y + explosionDiameter / 2.0;
        Ellipse explosion = new Ellipse();
        explosion.Fill = new SolidColorBrush(Colors.White);
        explosion.Height = explosionDiameter;
        explosion.Width = explosionDiameter;
        Canvas.SetLeft(explosion, left);
        Canvas.SetTop(explosion, top);
        explosionEllipses.Add(explosion);
        mainWindow.backgroundCanvas.Children.Add(explosion);
        explosionDiameter++;
        foreach(Missile enemyMissile in mainWindow.enemyMissiles)
        {
          if (enemyMissile.X2 > left && enemyMissile.X2 < right &&
            enemyMissile.Y2 > top && enemyMissile.Y2 < bottom)
          {
            mainWindow.backgroundCanvas.Children.Remove(enemyMissile.MissileLine);
            if (!enemyMissile.Exploded)
            {
              enemyMissile.Exploded = true;
              mainWindow.enemy_missiles_live--;
            }
          }
        }
      }
      else
      {
        foreach(Ellipse expEllipse in explosionEllipses)
        {
          mainWindow.backgroundCanvas.Children.Remove(expEllipse);
        }
      }
    }

  }
}
