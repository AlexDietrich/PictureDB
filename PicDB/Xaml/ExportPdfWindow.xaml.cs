using System.Windows;
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
