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
using PicDB.Models;

namespace PicDB
{
    /// <summary>
    /// Interaktionslogik für ExportPdfWindow.xaml
    /// </summary>
    public partial class ExportPdfWindow : Window
    {
        private MainWindowViewModel _controller;
        public ExportPdfWindow(MainWindowViewModel controller)
        {
            InitializeComponent();

            _controller = controller;
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var report = new PDFReport();
            report.PdfReport(Tags.Text);
        }
    }
}
