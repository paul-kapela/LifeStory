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
    /// Logika interakcji dla klasy NewMenu.xaml
    /// </summary>
    public partial class NewMenu : System.Windows.Controls.Page
    {
        public NewMenu()
        {
            InitializeComponent();
        }

        private void pathButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationWindows.folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pathTextBox.Text = ApplicationWindows.folderBrowserDialog.SelectedPath;
            }
        }

        private void createNewButton_Click(object sender, RoutedEventArgs e)
        {
            if (Tools.IsNameValid(nameTextBox.Text) && Tools.IsPasswordValid(passwordTextBox.Text, repeatPasswordTextBox.Text) && Tools.CheckPath(pathTextBox.Text))
            {
                ApplicationWindows.mainWindow.Hide();
                Program.DiaryFile = new Diary(nameTextBox.Text, passwordTextBox.Text, pathTextBox.Text);
                ApplicationWindows.diaryWindow = new DiaryWindow();
                ApplicationWindows.diaryWindow.Show();
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationWindows.mainWindow.mainWindowFrame.Navigate(ApplicationWindows.mainMenu);
        }
    }
}
