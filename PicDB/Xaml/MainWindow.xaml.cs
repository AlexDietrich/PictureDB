using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using BIF.SWE2.Interfaces.ViewModels;
using PicDB.Models;
using PicDB.ViewModels;
using PicDB.Xaml;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace PicDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _controller;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public MainWindow()
        {
            GlobalInformation.ReadConfigFile(); // Save information from the config file 
            _controller = new MainWindowViewModel();
            InitializeComponent();
            this.DataContext = _controller;
            Searchbar.Foreground = Brushes.DimGray;
            Searchbar.Text = "Search picture";
            log.Debug("Applikation erfolgreich gestartet.");
        }

        private void BtnSaveIPTC_Click(object sender, RoutedEventArgs e)
        {
            IPictureViewModel currentPicture = _controller.CurrentPicture;

            currentPicture.IPTC.Keywords = UI_IPTC_Keywords.Text;
            currentPicture.IPTC.ByLine = UI_IPTC_ByLine.Text;
            currentPicture.IPTC.CopyrightNotice = UI_IPTC_CopyrightNotice.Text;
            currentPicture.IPTC.Headline = UI_IPTC_Headline.Text;
            currentPicture.IPTC.Caption = UI_IPTC_Caption.Text;

            _controller.SaveCurrentPicture();
        }

        private void PictureSelection_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (PictureSelection.SelectedItem is PictureViewModel)
            {
                _controller.CurrentPicture = (PictureViewModel)PictureSelection.SelectedItem;
            }

        }

        private void Searchbar_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ((PictureListViewModel)_controller.List).ResetList();
            if (Searchbar.IsFocused)
            {
                ObservableCollection<IPictureViewModel> filteredList = new ObservableCollection<IPictureViewModel>();
                foreach (IPictureViewModel viewModel in _controller.List.List)
                {
                    if (viewModel.FileName.ToLower().Contains(Searchbar.Text.ToLower()))
                    {
                        filteredList.Add(viewModel);
                    }
                }
                ((PictureListViewModel)_controller.List).List = filteredList;
            }
        }
        private void Searchbar_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Searchbar.Text) && Searchbar.Foreground == Brushes.DimGray)
            {
                Searchbar.Text = string.Empty;
                Searchbar.Foreground = Brushes.Black;
            }
        }
        private void Searchbar_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Searchbar.Text))
            {
                Searchbar.Foreground = Brushes.DimGray;
                Searchbar.Text = "Search picture";
            }
        }

        public void MenuOptionChangeHomeFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = GlobalInformation.Path;
            fbd.ShowDialog();

            //if the path changed, overwrite config file and load pictures of the new folder
            if (fbd.SelectedPath != GlobalInformation.Path)
            {
                var oldLines = System.IO.File.ReadAllLines("config.txt");
                List<string> newLines = new List<string>();
                foreach (var line in oldLines)
                {
                    if (line.Contains("path,"))
                    {
                        string addLine = "path," + fbd.SelectedPath + "\\";
                        newLines.Add(addLine);
                    }
                    else
                    {
                        newLines.Add(line);
                    }
                }
                System.IO.File.WriteAllLines("config.txt", newLines);
                GlobalInformation.ReadConfigFile();
                ((PictureListViewModel)_controller.List).SyncAndUpdatePictureList();
            }
        }

        private void MenuEditCameras_Click(object sender, RoutedEventArgs e)
        {
            var cameraWindow = new CameraWindow(_controller);
            cameraWindow.Show();
        }
        private void MenuEditPhotographers_Click(object sender, RoutedEventArgs e)
        {
            var photographerWindow = new PhotographerWindow(_controller);
            photographerWindow.Show();
        }
        

        private void BtnSaveGeneralInfo_Click(object sender, RoutedEventArgs e)
        {
            var CameraViewmodel = (CameraViewModel)CameraBox.SelectedItem;

            var PhotographerViewModel = (PhotographerViewModel)PhotogrBox.SelectedItem;

            _controller.SaveGeneralInformation(CameraViewmodel, PhotographerViewModel);
        }

        private void MenuFileExportPdf_Click(object sender, RoutedEventArgs e)
        {
            var exportPdfWindow = new ExportPdfWindow(_controller);

            exportPdfWindow.Show();
        }

        private void BtnExportPdf_Click(object sender, RoutedEventArgs e)
        {
            var report = new PDFReport();

            report.PdfReport((PictureViewModel)_controller.CurrentPicture);
        }

    }
}
