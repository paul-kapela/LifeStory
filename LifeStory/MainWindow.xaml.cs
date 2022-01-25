using System;
using System.Windows;
using CancelEventArgs = System.ComponentModel.CancelEventArgs;

namespace LifeStory
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
