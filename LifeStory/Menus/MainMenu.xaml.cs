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

namespace LifeStory
{
    /// <summary>
    /// Logika interakcji dla klasy MainMenu.xaml
    /// </summary>
    public partial class MainMenu : System.Windows.Controls.Page
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationWindows.mainWindow.mainWindowFrame.Navigate(ApplicationWindows.openWindow);
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationWindows.mainWindow.mainWindowFrame.Navigate(ApplicationWindows.newWindow);
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationWindows.mainWindow.mainWindowFrame.Navigate(ApplicationWindows.aboutMenu);
        }
    }
}
