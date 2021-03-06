﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using ListBox = System.Windows.Controls.ListBox;
using ArticleDBLib;
using ArticleDBLib.Models;

namespace ArticleViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Zmienna przechowująca wybrany artykuł z listy
        public static Articles SelectedArticle { get; set; }

        public static Comments SelectedComment { get; set; }

        //Umożliwia dostęp do głównego okna z innych okien
        public static MainWindow AppWindow;

        // Wczytanie artykułów do listy
        public void LoadArticlesToList()
        {
            lstBox.ItemsSource = DbDataAccess.LoadArticleList();
        }

        // Wczytanie komentarzy do listy
        public void LoadCommentToList()
        {
            commentsBox.ItemsSource = DbDataAccess.GetCommentsToArticles(SelectedArticle.Id);
        }

        public MainWindow()
        {
            InitializeComponent();
            AppWindow = this;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            DbDataAccess.CreateDB();

            // Wczytanie artykułów do listy
            LoadArticlesToList();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ListBox)sender;

            // Zapisanie wybranego artykułu do zmiennej
            SelectedArticle = (Articles)item.SelectedItem;
            SelectedArticle.File = new Files { Filename = DbDataAccess.GetFileName(SelectedArticle.Id) };
            fileNameBox.Text = SelectedArticle.File.Filename;

            OpenFileBtn.IsEnabled = true;
            AddComm.IsEnabled = true;

            LoadCommentToList();
        }

        private void OpenFileBtn_Click(object sender, RoutedEventArgs e)
        {
            PDF_viewer pdf_viever = new PDF_viewer();
            pdf_viever.Show();
        }
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            var ExportWindow = new Import();
            ExportWindow.Show();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var ExportWindow = new Export();
            ExportWindow.Show();
        }

        private void AddComm_Click(object sender, RoutedEventArgs e)
        {
            var AddCommWindow = new AddComment();
            AddCommWindow.Show();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var AddWindow = new Info();
            AddWindow.Show();
        }

        private void DelComm_Click(object sender, RoutedEventArgs e)
        {
            DbDataAccess.DeleteComment(SelectedComment.Id);
            LoadCommentToList();
            DelComm.IsEnabled = false;
        }

        private void commentsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ListBox)sender;
            SelectedComment = (Comments)item.SelectedItem;
            DelComm.IsEnabled = true;
        }
    }
}
