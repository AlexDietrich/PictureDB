using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            throw new NotImplementedException();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Camerabox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var CameraModel = (CameraViewModel)CameraBox.SelectedItem;

            Producer.Text = CameraModel.Producer;
            Make.Text = CameraModel.Make;
            BoughtOn.Text = CameraModel.BoughtOn.ToString();
            Notes.Text = CameraModel.Notes;
            ISOLimitGood.Text = CameraModel.ISOLimitGood.ToString();
            ISOLimitAcceptable.Text = CameraModel.ISOLimitAcceptable.ToString();
        }
    }
}
