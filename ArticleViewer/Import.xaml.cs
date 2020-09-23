using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.IO;
using System.Security.Cryptography;
using ArticleDBLib.Models;
using ArticleDBLib;
using System.Windows.Navigation;
using System.Text.RegularExpressions;
using Syncfusion.Windows.PdfViewer;
using Syncfusion.XPS;
using System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace ArticleViewer
{
    /// <summary>
    /// Interaction logic for Import.xaml
    /// </summary>
    public partial class Import : Window
    {
        private string filePath = string.Empty;
        private List<string> Lines = new List<string>();
        private List<Authors> Auts = new List<Authors>();
        private List<Articles> Articles = new List<Articles>();
        public Import()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            
        }
        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "BibTex file (*.bib)|*.bib";
            if (o.ShowDialog().ToString().Equals("OK"))
            {
                filePath = o.FileName;
                src.Text = filePath;
            }
        }
        
        private void Load(string f)
        {
            using (StreamReader reader = new StreamReader(f))
            {
                string line;
                while ((line=reader.ReadLine()) != null)
                {
                    Lines.Add(line);
                }
            }
        }
        private bool Check(List<string> l)
        {
            int count = 0;
            foreach (string line in l)
            {
                if (line.StartsWith("@article"))
                {
                    count++;
                }
            }
            if (count > 0)
                return true;
            else
                return false;
        }
        private void Upload(List<string> l)
        {
            string uAuthor = "Not found";
            string uTitle = "Not found";
            string uJournal = "Not found";
            int uYear = 0000;
            int uNumber = 0;
            int uPages = 0;
            int uVolume = 0;
            foreach(string line in l)
            {
                if (line.StartsWith("\tauthor", StringComparison.InvariantCultureIgnoreCase))
                {
                    uAuthor = line.Substring(line.LastIndexOf("{")+1, (line.LastIndexOf("}") - line.LastIndexOf("{")-1));
                }
                else if (line.StartsWith("\ttitle", StringComparison.InvariantCultureIgnoreCase))
                {
                    uTitle = line.Substring(line.LastIndexOf("{")+1, (line.LastIndexOf("}") - line.LastIndexOf("{")-1));
                }
                else if (line.StartsWith("\tjournal", StringComparison.InvariantCultureIgnoreCase))
                {
                    uJournal = line.Substring(line.LastIndexOf("{")+1, (line.LastIndexOf("}") - line.LastIndexOf("{")-1));
                }
                else if (line.StartsWith("\tyear", StringComparison.InvariantCultureIgnoreCase))
                {
                    uYear = int.Parse(line.Substring(line.LastIndexOf("=")+2,4));
                }
                else if (line.StartsWith("\tnumber", StringComparison.InvariantCultureIgnoreCase))
                {
                    uNumber = int.Parse(line.Substring(line.LastIndexOf("=") + 2,1));
                }
                else if (line.StartsWith("\tpages", StringComparison.InvariantCultureIgnoreCase))
                {
                    uPages = int.Parse(line.Substring(line.LastIndexOf("{")+1, (line.LastIndexOf("}") - line.LastIndexOf("{")-1)));
                }
                else if (line.StartsWith("\tvolume", StringComparison.InvariantCultureIgnoreCase))
                {
                    uVolume = int.Parse(line.Substring(line.LastIndexOf("=") + 2,1));
                }
                
            }
            Articles = DbDataAccess.LoadArticleList();
            int index = Articles.FindIndex(item => item.Title == uTitle);
            if (index >= 0)
            {
                System.Windows.MessageBox.Show("Article with this title already exist in database!", "File already exist");
            }
            else
            {
                Articles a = new Articles()
                {
                    Title = uTitle,
                    Journal = uJournal,
                    Year = uYear,
                    Number = uNumber,
                    Pages = uPages,
                    Volume = uVolume
                };
                DbDataAccess.SaveArticle(a);
                Authors aut = new Authors() { Author = uAuthor };
                Auts.Add(aut);
                System.Windows.MessageBox.Show($"{Auts[0].Author}");
                DbDataAccess.SaveAuthor(aut);
                DbDataAccess.MatchAuthorsToArticles(DbDataAccess.GetLastArticleId(), Auts);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void import_Click(object sender, RoutedEventArgs e)
        {
            Load(filePath);
            if (Lines is null)
            {
                System.Windows.MessageBox.Show("Failed to load the file!","Invalid File");
            }
            else if (Check(Lines) == true)
            {
                Upload(Lines);
            }
            else
                System.Windows.MessageBox.Show("File is not an Article BibTeX entry!","Invalid File");
            this.Close();
        }
    }
}
