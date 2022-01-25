using System.Windows;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace LifeStory
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    ///

    public static class ApplicationWindows
    {
        public static MainWindow mainWindow = new MainWindow();
        public static MainMenu mainMenu = new MainMenu();
        public static OpenMenu openWindow = new OpenMenu();
        public static NewMenu newWindow = new NewMenu();
        public static AboutMenu aboutMenu = new AboutMenu();
        public static DiaryWindow diaryWindow;
        public static FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        public static OpenFileDialog openFileDialog = new OpenFileDialog();
        public static SaveFileDialog saveFileDialog = new SaveFileDialog();
    }

    public partial class App : Application
    {
        public static string Name { get { return "LifeStory"; } }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ApplicationWindows.mainWindow.mainWindowFrame.Navigate(ApplicationWindows.mainMenu);
            ApplicationWindows.mainWindow.Show();
        }
    }
}
