using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using Newtonsoft.Json;
using System.Windows.Controls.Primitives;
using System.Security.Permissions;
using System.Security;

namespace LifeStory
{
    // TODO: Aplikacja ma mieć możliwość zmiany rozmiaru okna
    // TODO: Coś jest nie tak ze sprawdzaniem uprawnień do danej ścieżki

    public static class Tools
    {
        public static bool IsNameValid(string name)
        {
            char[] unallowedCharacters = new char[9] { (char)92, (char)47, (char)58, (char)42, (char)63, (char)34, (char)60, (char)62, (char)124 };

            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Nie podano nazwy.", "LifeStory", MessageBoxButton.OK);
                return false;
            }
            else if (ContainsCharacters(name, unallowedCharacters))
            {
                MessageBox.Show(new StringBuilder(@"Nazwa nie może zawierać jednego z następujących znaków: ").Append(unallowedCharacters[0]).Append(unallowedCharacters[1]).Append(unallowedCharacters[2]).Append(unallowedCharacters[3]).Append(unallowedCharacters[4]).Append(unallowedCharacters[5]).Append(unallowedCharacters[6]).Append(unallowedCharacters[7]).Append(unallowedCharacters[8]).Append(" Sprawdź ją i spróbuj ponownie.").ToString(), "LifeStory", MessageBoxButton.OK);
                return false;
            }

            return true;
        }

        public static bool IsPasswordEmpty(string password)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Nie podano hasła. Sprawdź je i spróbuj ponownie.", "LifeStory", MessageBoxButton.OK);
                return true;
            }

            return false;
        }

        public static bool IsPasswordValid(string password, string confirmPassword)
        {
            if ((string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password)) || (string.IsNullOrEmpty(confirmPassword) || string.IsNullOrWhiteSpace(confirmPassword)))
            {
                MessageBox.Show("Nie podano hasła. Sprawdź je i spróbuj ponownie.", "LifeStory", MessageBoxButton.OK);
                return false;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Potwierdzenie hasła nie zgadza się z podanym hasłem. Sprawdź je i spróbuj ponownie.", "LifeStory", MessageBoxButton.OK);
                return false;
            }

            return true;
        }

        public static bool IsPathValid(string path)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show("Nie podano ścieżki. Sprawdź ją  i spróbuj ponownie.", "LifeStory", MessageBoxButton.OK);
                return false;
            }
            else if (!Directory.Exists(path))
            {
                if (!File.Exists(path))
                {
                    MessageBox.Show("Podana ścieżka nie istnieje. Sprawdź ją i spróbuj ponownie.", "LifeStory", MessageBoxButton.OK);
                    return false;
                }     
            }
            else if (!HasWriteAccessToFolder(path))
            {
                MessageBox.Show("Nie masz uprawnień do podanej ścieżki. Sprawdź ją i spróbuj ponownie.", "LifeStory", MessageBoxButton.OK);
                return false;
            }

            return true;
        }

        public static bool ContainsCharacters(string toCheck, char[] characters)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                if (toCheck.Contains(characters[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasWriteAccessToFolder(string path)
        {
            try
            {
                using (FileStream fs = File.Create(
                    Path.Combine(
                        path,
                        Path.GetRandomFileName()
                    ),
                    1,
                    FileOptions.DeleteOnClose)
                )
                { }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckPath(string path)
        {
            return IsPathValid(path) && HasWriteAccessToFolder(path);
        }

        public static string GetRtf(RichTextBox richTextBox)
        {
            string result = string.Empty;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                textRange.Save(memoryStream, DataFormats.Rtf);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    result = streamReader.ReadToEnd();
                }
            }

            return result;
        }

        public static void LoadRtf(RichTextBox richTextBox, string toLoad)
        {
            richTextBox.Document = new FlowDocument();

            if (toLoad == string.Empty)
            {
                return;
            }
            else
            {
                using (MemoryStream memoryStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(toLoad)))
                {
                    richTextBox.Selection.Load(memoryStream, DataFormats.Rtf);
                }
            }
        }
    }

    public static class Program
    {
        public static Diary DiaryFile;

        public static string GetWindowTitle()
        {
            return new StringBuilder("LifeStory - ").Append(DiaryFile.Name).ToString();
        }

        public static string GetWindowTitle(int index)
        {
            return new StringBuilder("LifeStory - ").Append(DiaryFile.Name).Append(": ").Append(DiaryFile.Pages[index].Name).ToString();
        }

        public static Point GetCursorPosition(RichTextBox richTextBox)
        {
            TextPointer textPointer1 = richTextBox.Selection.Start.GetLineStartPosition(0);
            TextPointer textPointer2 = richTextBox.Selection.Start;

            int column = textPointer1.GetOffsetToPosition(textPointer2);
            int someBigNumber = int.MaxValue;
            int lineMoved, line;

            richTextBox.Selection.Start.GetLineStartPosition(-someBigNumber, out lineMoved);
            line = -lineMoved;

            return new Point(line, column);
        }
    }

    public class Diary
    {
        public bool Loaded = false;
        public bool AreChangesSaved = true; //do tego zdarzenie przy zmianie wartości tej zmiennej

        public string Name;
        public string Password;
        public int KeyLength;
        public string Key;
        public string KeyHash;
        public string Path;
        public string FolderPath;
        public DateTime CreateTime;
        public DateTime ModificateTime;
        public ObservableCollection<Page> Pages;

        public int currentlyOpenedPageIndex = -1;

        /// <summary>Creates a new diary</summary>
        public Diary(string name, string password, string path)
        {
            DateTime now = DateTime.Now;

            Loaded = true;

            Name = name;
            Password = password;
            KeyLength = 128;
            Key = Cryptography.RandomString(KeyLength);
            KeyHash = Cryptography.GenerateHash(Key);
            Path = new StringBuilder(path).Append(@"\").Append(name).Append(".ls").ToString();
            FolderPath = path;
            CreateTime = now;
            ModificateTime = now;

            Pages = new ObservableCollection<Page>();
        }

        /// <summary>Open an existing diary</summary>
        public Diary(string password, string path)
        {
            DataTemplate dataTemplate;

            using (StreamReader streamReader = new StreamReader(path))
            { 
                dataTemplate = JsonConvert.DeserializeObject<DataTemplate>(streamReader.ReadToEnd());
            }

            Name = dataTemplate.Name;

            string key;

            try
            {
                key = Cryptography.Decrypt(dataTemplate.Key, password);

                if (Cryptography.ValidateDecipheredString(key, dataTemplate.KeyHash))
                    Key = key;
            }
            catch { }

            if (string.IsNullOrEmpty(Key))
            {
                MessageBox.Show("Złe hasło", App.Name);
                return;
            }

            Password = password;
            KeyHash = dataTemplate.KeyHash;
            Path = path;
            FolderPath = path.Trim(path.Substring(path.LastIndexOf(@"\")).ToCharArray());
            CreateTime = dataTemplate.CreateTime;
            ModificateTime = dataTemplate.ModificateTime;

            Pages = new ObservableCollection<Page>();

            foreach (Page cipheredPage in dataTemplate.CipheredPages)
            {
                Page page = new Page(Cryptography.Decrypt(cipheredPage.Name, Key), cipheredPage.Content, cipheredPage.CreateTime, cipheredPage.ModificateTime);
                Pages.Add(page);
            }

            Loaded = true;
        }

        public void AddPage()
        {
            Pages.Add(new Page());
        }

        /// <summary>Save current page (write a RTF string to the currently opened page)</summary>
        public void SavePage(string pageRtf)
        {
            if (!(Pages[currentlyOpenedPageIndex].Content == pageRtf))
            {
                Pages[currentlyOpenedPageIndex].ModificateTime = DateTime.Now;
                Pages[currentlyOpenedPageIndex].Content = pageRtf;
            }

            AreChangesSaved = false;
        }

        /// <summary>Load a page at to the given RichTextBox</summary>
        public void LoadPage(int index, RichTextBox richTextBox)
        {
            Tools.LoadRtf(richTextBox, Cryptography.Decrypt(Pages[index].Content, Key));
            currentlyOpenedPageIndex = index;
        }

        public void RemovePage(int index)
        {
            Pages.RemoveAt(index);
        }

        public void Save()
        {
            if (Tools.HasWriteAccessToFolder(FolderPath))
            {
                List<Page> cipheredPages = new List<Page>();

                foreach (Page page in Pages)
                {
                    Page cipheredPage = new Page(Cryptography.Encrypt(page.Name, Key), Cryptography.Encrypt(page.Content, Key), page.CreateTime, page.ModificateTime);
                    cipheredPages.Add(cipheredPage);
                }

                DataTemplate dataTemplate = new DataTemplate(Name, Cryptography.Encrypt(Key, Password), KeyHash, CreateTime, ModificateTime, cipheredPages);
                File.WriteAllText(Path, JsonConvert.SerializeObject(dataTemplate));

                AreChangesSaved = true;
            }
            else
            {
                MessageBox.Show("Nie można zapisać pliku!", App.Name);
            }
        }
    }

    public class DataTemplate
    {
        [JsonProperty("Name")]
        public string Name;
        [JsonProperty("Key")]
        public string Key;
        [JsonProperty("KeyHash")]
        public string KeyHash;
        [JsonProperty("CreateTime")]
        public DateTime CreateTime;
        [JsonProperty("ModificateTime")]
        public DateTime ModificateTime;
        [JsonProperty("CipheredPages")]
        public List<Page> CipheredPages;

        public DataTemplate(string name, string key, string keyHash, DateTime createTime, DateTime modificateTime, List<Page> cipheredPages)
        {
            Name = name;
            Key = key;
            KeyHash = keyHash;
            CreateTime = createTime;
            ModificateTime = modificateTime;
            CipheredPages = cipheredPages;
        }
    }

    public class Page
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Content")]
        public string Content { get; set; }
        [JsonProperty("CreateTime")]
        public DateTime CreateTime { get; set; }
        [JsonProperty("ModificateTime")]
        public DateTime ModificateTime { get; set; }

        public Page()
        {
            DateTime now = DateTime.Now;
            Name = "Nowa kartka";
            Content = string.Empty;
            CreateTime = now;
            ModificateTime = now;
        }

        public Page(string name, string content, DateTime createTime, DateTime modificateTime)
        {
            Name = name;
            Content = content;
            CreateTime = createTime;
            ModificateTime = modificateTime;
        }
    }
}
