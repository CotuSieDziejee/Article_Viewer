using Syncfusion.Windows.PdfViewer;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace ArticleViewer
{
    /// <summary>
    /// Interaction logic for PDF_viewer.xaml
    /// </summary>
    public partial class PDF_viewer : Window
    {
        public PDF_viewer()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            pdfViewerControl.Load(System.Environment.CurrentDirectory + "\\Articles\\" + MainWindow.SelectedArticle.File.Filename);
        }
    }
}
