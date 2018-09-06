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

namespace PicDB.Xaml
{
    /// <summary>
    /// Interaktionslogik für PhotographerAddWindow.xaml
    /// </summary>
    public partial class PhotographerAddWindow : Window
    {
        private MainWindowViewModel Controller { get; set; } = null;
        public PhotographerAddWindow(MainWindowViewModel controller)
        {
            Controller = controller;
            InitializeComponent();
            this.DataContext = Controller;
        }

        private void SaveBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var photographer = new PhotographerViewModel();
            var birthday = DateTime.Now;

            if (DateTime.TryParse(Birthday.Text, out birthday))
            {
                if (birthday < DateTime.Now && !string.IsNullOrWhiteSpace(FirstName.Text) && !string.IsNullOrWhiteSpace(LastName.Text) && !string.IsNullOrWhiteSpace(Notes.Text))
                {
                    photographer.BirthDay = birthday;
                    photographer.FirstName = FirstName.Text;
                    photographer.LastName = LastName.Text;
                    photographer.Notes = Notes.Text;

                    Controller.SavePhotographer(photographer);
                    this.Close();
                }
                else
                {
                    ErrorLabel.Content = "INVALID INPUT";
                }
            }
            else
            {
                ErrorLabel.Content = "INVALID INPUT";
            }

        }
    }
}
