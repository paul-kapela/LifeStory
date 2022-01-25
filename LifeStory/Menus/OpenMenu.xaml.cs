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
    /// Logika interakcji dla klasy OpenMenu.xaml
    /// </summary>
    public partial class OpenMenu : System.Windows.Controls.Page
    {
        public OpenMenu()
        {
            InitializeComponent();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationWindows.mainWindow.mainWindowFrame.Navigate(ApplicationWindows.mainMenu);
        }

        private void pathButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationWindows.openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pathTextBox.Text = ApplicationWindows.openFileDialog.InitialDirectory + ApplicationWindows.openFileDialog.FileName;
            }
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            if (Tools.IsPathValid(pathTextBox.Text) && !Tools.IsPasswordEmpty(passwordTextBox.Text))
            {
                Program.DiaryFile = new Diary(passwordTextBox.Text, pathTextBox.Text);

                if (Program.DiaryFile.Loaded)
                {
                    ApplicationWindows.mainWindow.Hide();
                    ApplicationWindows.diaryWindow = new DiaryWindow();
                    ApplicationWindows.diaryWindow.Show();
                }
            }
        }
    }
}
