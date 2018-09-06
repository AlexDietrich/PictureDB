using PicDB.Models;
using System;
using System.Windows;
using PicDB.ViewModels;

namespace PicDB
{
    /// <summary>
    /// Interaction logic for CameraAddWindow.xaml
    /// </summary>
    public partial class CameraAddWindow : Window
    {
        private MainWindowViewModel _controller;
        public CameraAddWindow(MainWindowViewModel controller)
        {
            _controller = controller;
            InitializeComponent();
            this.DataContext = _controller;
        }


        private void SaveBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var camera = new CameraViewModel();     
            var boughtOn = DateTime.Now;
            decimal isoLimits;

            if (DateTime.TryParse(BoughtOn.Text, out boughtOn)) camera.BoughtOn = boughtOn; 
            if (!string.IsNullOrWhiteSpace(Producer.Text)) camera.Producer = Producer.Text;
            if (!string.IsNullOrWhiteSpace(Make.Text)) camera.Make = Make.Text;
            if (!string.IsNullOrWhiteSpace(Notes.Text)) camera.Notes = Notes.Text;

            if (!string.IsNullOrWhiteSpace(ISOLimitAcceptable.Text))
                if (decimal.TryParse(ISOLimitAcceptable.Text, out isoLimits))
                camera.ISOLimitAcceptable = isoLimits;

            if (!string.IsNullOrWhiteSpace(ISOLimitGood.Text))
                if (decimal.TryParse(ISOLimitGood.Text, out isoLimits))
                    camera.ISOLimitGood = isoLimits; 
            

            _controller.SaveCamera(camera);
            this.Close();
        }
    }
}
