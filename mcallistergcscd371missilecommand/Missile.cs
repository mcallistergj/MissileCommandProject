using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace mcallistergcscd371missilecommand
{
  class Missile
  {
    private double fireAngle;
    private Line missileLine;
    private Point target;
    private bool exploded;

    public Missile()
    {
      missileLine = new Line();
      missileLine.Stroke = Brushes.White;
      exploded = false;
    }

    public double X1
    {
      get { return missileLine.X1; }
      set { missileLine.X1 = value; }
    }

    public double X2
    {
      get { return missileLine.X2; }
      set { missileLine.X2 = value; }
    }

    public double Y1
    {
      get { return missileLine.Y1; }
      set { missileLine.Y1 = value; }
    }

    public double Y2
    {
      get { return missileLine.Y2; }
      set { missileLine.Y2 = value; }
    }

    public double FireAngle
    {
      get { return fireAngle; }
      set { fireAngle = value; }
    }

    public Line MissileLine
    {
      get { return missileLine; }
    }

    public Point Target
    {
      get { return target; }
      set { target = value; }
    }

    public bool Exploded
    {
      get { return exploded; }
      set { exploded = value; }
    }

    public void detonate(MainWindow mainWindow)
    {
      Explosion explosion = new Explosion(mainWindow, new Point(X2, Y2), this);
      this.exploded = true;
    }



  }
}
