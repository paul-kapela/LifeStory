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
using System.ComponentModel;

namespace LifeStory
{
    // TODO: Blok edycji tekstu we wstążce ma być wyłączony, jeśli nie ma żadnej aktywnej kartki
    // TODO: Kontrolki od edycji tekstu mają wskazywać aktualne cechy tekstu (czcionka, krój, itd)
    // TODO: Dodawanie obrazków
    // TODO: Zapisz przy wychodzeniu z programu (zapytanie)
    // TODO: Autozapis
    // TODO: Wskaźnik, czy plik posiada zmiany, które mogą zostać zapisane

    /// <summary>
    /// Logika interakcji dla klasy DiaryWindow.xaml
    /// </summary>
    /// 
    public partial class DiaryWindow : Window
    {
        public DiaryWindow()
        {
            InitializeComponent();
            Title = new StringBuilder("LifeStory - ").Append(Program.DiaryFile.Name).ToString();
            pagesListBox.ItemsSource = Program.DiaryFile.Pages;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Program.DiaryFile.AreChangesSaved)
                Environment.Exit(0);
            else
            {
                switch (MessageBox.Show("Plik posiada niezapisane zmiany. Czy chcesz go zapisać przed zamknięciem programu?", App.Name, MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
                {
                    case MessageBoxResult.Yes:
                        Save();
                        Environment.Exit(0);
                        break;

                    case MessageBoxResult.No:
                        Environment.Exit(0);
                        break;

                    case MessageBoxResult.Cancel:
                        return;
                }
            }
        }

        private void SwitchPage(object sender, int index)
        {
            if (index == -1 || pagesListBox.SelectedItems.Count != 1)
            {
                if (Program.DiaryFile.Pages.Count != 0 && Program.DiaryFile.currentlyOpenedPageIndex != -1)
                    Program.DiaryFile.SavePage(Tools.GetRtf(pageRichTextBox));

                Title = Program.GetWindowTitle();
                pageRichTextBox.Document.Blocks.Clear();
                pageRichTextBox.IsEnabled = false;
                Program.DiaryFile.currentlyOpenedPageIndex = -1;
            }
            else
            {
                if (Program.DiaryFile.currentlyOpenedPageIndex == -1)
                {
                    Title = Program.GetWindowTitle(index);
                    Program.DiaryFile.LoadPage(index, pageRichTextBox);
                    pageRichTextBox.IsEnabled = true;
                }
                else
                {
                    Program.DiaryFile.SavePage(Tools.GetRtf(pageRichTextBox));
                    Title = Program.GetWindowTitle(index);
                    Program.DiaryFile.LoadPage(index, pageRichTextBox);
                    pageRichTextBox.IsEnabled = true;
                }
            }
        }

        private void Save()
        {
            if (Program.DiaryFile.Pages.Count != 0)
            {
                Program.DiaryFile.SavePage(Tools.GetRtf(pageRichTextBox));
            }

            Program.DiaryFile.ModificateTime = DateTime.Now;
            Program.DiaryFile.Save();
        }

        private void pageNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(Program.DiaryFile.Pages[Program.DiaryFile.currentlyOpenedPageIndex].Name == (sender as TextBox).Text))
            {
                Program.DiaryFile.Pages[Program.DiaryFile.currentlyOpenedPageIndex].ModificateTime = DateTime.Now;
                Program.DiaryFile.Pages[Program.DiaryFile.currentlyOpenedPageIndex].Name = (sender as TextBox).Text;
                Title = string.Format("LifeStory - {0} - {1}", Program.DiaryFile.Name, Program.DiaryFile.Pages[Program.DiaryFile.currentlyOpenedPageIndex].Name);
            }
        }

        private void newPageButton_Click(object sender, RoutedEventArgs e)
        {
            Program.DiaryFile.AddPage();
        }

        private void deletePageButton_Click(object sender, RoutedEventArgs e)
        {
            if (pagesListBox.SelectedIndex != -1)
            {
                if (MessageBox.Show("Czy na pewno chcesz usunąć tę kartkę?", "LifeStory", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    while (pagesListBox.SelectedItems.Count > 0)
                    {
                        
                        // TODO: Napraw usuwanie wielu kartek (usuń zaznaczenie po uprzednim pobraniu go)
                        Program.DiaryFile.RemovePage(pagesListBox.SelectedIndex);
                    }
                }
            }
        }

        private void pagesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SwitchPage(sender, pagesListBox.SelectedIndex);
        }

        private void alignLeftButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextRange currentTextRange = new TextRange(pageRichTextBox.Selection.Start, pageRichTextBox.Selection.End);
                currentTextRange.ApplyPropertyValue(TextBlock.TextAlignmentProperty, TextAlignment.Left);
            }
            catch { }
        }

        private void alignCenterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextRange currentTextRange = new TextRange(pageRichTextBox.Selection.Start, pageRichTextBox.Selection.End);
                currentTextRange.ApplyPropertyValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
            }
            catch { }
        }

        private void alignRightButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextRange currentTextSelection = new TextRange(pageRichTextBox.Selection.Start, pageRichTextBox.Selection.End);
                currentTextSelection.ApplyPropertyValue(TextBlock.TextAlignmentProperty, TextAlignment.Right);
            }
            catch { }
        }

        private void boldTextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextRange currentTextSelection = new TextRange(pageRichTextBox.Selection.Start, pageRichTextBox.Selection.End);

                if (!(string.IsNullOrEmpty(currentTextSelection.Text) || string.IsNullOrWhiteSpace(currentTextSelection.Text)))
                {
                    if (!(currentTextSelection.GetPropertyValue(TextElement.FontWeightProperty).Equals(FontWeights.Bold)))
                    {
                        currentTextSelection.ApplyPropertyValue(TextBlock.FontWeightProperty, FontWeights.Bold);
                    }
                    else
                    {
                        currentTextSelection.ApplyPropertyValue(TextBlock.FontWeightProperty, FontWeights.Normal);
                    }
                }
            }
            catch { }
        }

        private void italicTextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextRange currentTextSelection = new TextRange(pageRichTextBox.Selection.Start, pageRichTextBox.Selection.End);

                if (!(string.IsNullOrEmpty(currentTextSelection.Text) || string.IsNullOrWhiteSpace(currentTextSelection.Text)))
                {
                    if (!(currentTextSelection.GetPropertyValue(TextElement.FontStyleProperty).Equals(FontStyles.Italic)))
                    {
                        currentTextSelection.ApplyPropertyValue(TextBlock.FontStyleProperty, FontStyles.Italic);
                    }
                    else
                    {
                        currentTextSelection.ApplyPropertyValue(TextBlock.FontStyleProperty, FontStyles.Normal);
                    }
                }
            }
            catch { }
        }

        private void underlineTextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextRange currentTextSelection = new TextRange(pageRichTextBox.Selection.Start, pageRichTextBox.Selection.End);

                if (string.IsNullOrEmpty(currentTextSelection.Text) || string.IsNullOrWhiteSpace(currentTextSelection.Text))
                {
                    currentTextSelection = new TextRange(pageRichTextBox.Selection.End, pageRichTextBox.Selection.End);
                }

                if (!currentTextSelection.GetPropertyValue(Inline.TextDecorationsProperty).Equals(TextDecorations.Underline))
                {
                    currentTextSelection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
                }
                else
                {
                    currentTextSelection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
                }
            }
            catch { }
        }

        private void fontSelectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TextRange currentTextSelection = new TextRange(pageRichTextBox.Selection.Start, pageRichTextBox.Selection.End);

                if (!(string.IsNullOrEmpty(currentTextSelection.Text) || string.IsNullOrWhiteSpace(currentTextSelection.Text)))
                {
                    currentTextSelection.ApplyPropertyValue(Inline.FontFamilyProperty, fontSelectionComboBox.SelectedItem);
                }
            }
            catch { }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void pageRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (pageRichTextBox.IsEnabled == true)
            {
                Point currentCursorPosition = Program.GetCursorPosition(pageRichTextBox);
                
                cursorPositionTextBlock.Text = string.Format("Linia {0}, kolumna {1}", currentCursorPosition.X, currentCursorPosition.Y);
            }
            else
            {
                cursorPositionTextBlock.Text = string.Empty;
            }
        }

        private void BulletedListButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void IncreaseFontSizeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DecreaseFontSizeButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
