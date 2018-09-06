using System;
using System.Windows;
using System.Windows.Controls;
using PicDB.Models;
using PicDB.ViewModels;

namespace PicDB
{
    /// <summary>
    /// Interaction logic for CameraWindow.xaml
    /// </summary>
    public partial class CameraWindow : Window
    {
        private MainWindowViewModel _controller;
        private CameraViewModel lastSelectedViewModel;

        public CameraWindow(MainWindowViewModel controller)
        {
            _controller = controller;
            InitializeComponent();
            this.DataContext = _controller;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var CameraAddWindow = new CameraAddWindow(_controller);
            CameraAddWindow.Show();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lastSelectedViewModel != null)
            {
                var cameraViewModel = lastSelectedViewModel;
                int ID = cameraViewModel.ID;
                try
                {
                    _controller.DeleteCamera(ID);
                }
                catch
                {
                    MessageBox.Show("Can't Delete This Camera Because it its assigned to a Picture", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                Producer.Text = string.Empty;
                Make.Text = string.Empty;
                BoughtOn.Text = string.Empty;
                Notes.Text = string.Empty;
                ISOLimitGood.Text = string.Empty;
                ISOLimitAcceptable.Text = string.Empty;
            }
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (lastSelectedViewModel == null) return;
            CameraViewModel cameraViewModel = lastSelectedViewModel;

            cameraViewModel.Producer = Producer.Text;
            cameraViewModel.Make = Make.Text;
            cameraViewModel.BoughtOn = DateTime.Parse(BoughtOn.Text);
            cameraViewModel.Notes = Notes.Text;
            Decimal.TryParse(ISOLimitGood.Text, out decimal isoLimitGood);
            cameraViewModel.ISOLimitGood = isoLimitGood;
            Decimal.TryParse(ISOLimitAcceptable.Text, out decimal isoLimitAcceptable);
            cameraViewModel.ISOLimitAcceptable = isoLimitAcceptable;

            _controller.UpdateCamera(cameraViewModel);
        }

        private void Camerabox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CameraBox.SelectedItem == null) return;
            var CameraModel = (CameraViewModel)CameraBox.SelectedItem;
            lastSelectedViewModel = CameraModel;

            Producer.Text = CameraModel.Producer;
            Make.Text = CameraModel.Make;
            BoughtOn.Text = CameraModel.BoughtOn.ToString();
            Notes.Text = CameraModel.Notes;
            ISOLimitGood.Text = CameraModel.ISOLimitGood.ToString();
            ISOLimitAcceptable.Text = CameraModel.ISOLimitAcceptable.ToString();
        }
    }
}
