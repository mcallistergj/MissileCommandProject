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
using System.Windows.Shapes;

namespace mcallistergcscd371missilecommand
{
  /// <summary>
  /// Interaction logic for OptionsWindow.xaml
  /// </summary>
  public partial class OptionsWindow : Window
  {
    MainWindow mainWindow;

    public OptionsWindow(MainWindow main)
    {
      InitializeComponent();
      mainWindow = main;
    }

    private void missileCountSelected(object sender, RoutedEventArgs e)
    {
      if(standardRadio.IsChecked == true)
      {
        mainWindow.number_of_defense_missiles = 30;
        customCountTextBox.Clear();
        customCountTextBox.IsEnabled = false;
      }
      else if(infiniteRadio.IsChecked == true)
      {
        mainWindow.number_of_defense_missiles = 1000;
        customCountTextBox.Clear();
        customCountTextBox.IsEnabled = false;
      }
      else if(customCountRadio.IsChecked == true)
      {
        customCountTextBox.IsEnabled = true;
        acceptButton.IsEnabled = false;
      }
    }

    private void checkCustomMissileText(object sender, TextChangedEventArgs e)
    {
      if (int.TryParse(customCountTextBox.Text, out mainWindow.number_of_defense_missiles))
      {
        customCountTextBox.Background = Brushes.LightGreen;
        acceptButton.IsEnabled = true;
      }
      else
      {
        customCountTextBox.Background = Brushes.PaleVioletRed;
        acceptButton.IsEnabled = false;
      }
    }

    private void cancelButton_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void citiesEntered(object sender, TextChangedEventArgs e)
    {
      if(int.TryParse(citiesTextBox.Text, out mainWindow.cities_to_defend))
      {
        citiesTextBox.Background = Brushes.LightGreen;
        acceptButton.IsEnabled = true;
      }
      else
      {
        citiesTextBox.Background = Brushes.PaleVioletRed;
        acceptButton.IsEnabled = false;
      }
    }

    private void missileSpeedChecked(object sender, RoutedEventArgs e)
    {
      if(constantRadio.IsChecked == true)
      {
        mainWindow.increasing_missile_speed = false;
      }
      else
      {
        mainWindow.increasing_missile_speed = true;
      }
    }

    private void acceptButtonClicked(object sender, RoutedEventArgs e)
    {
      if (!citiesTextBox.Text.Equals(""))
      {
        mainWindow.cities_to_defend = int.Parse(citiesTextBox.Text);
      }
      mainWindow.start_game();
      this.Close();
    }
  }
}
