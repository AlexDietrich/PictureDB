using System;
using System.Windows;
using System.Windows.Controls;
using PicDB.Models;
using PicDB.ViewModels;

namespace PicDB.Xaml
{
    /// <summary>
    /// Interaktionslogik für PhotographerWindow.xaml
    /// </summary>
    public partial class PhotographerWindow : Window
    {
        private MainWindowViewModel Controller { get; set; } = null;
        private PhotographerViewModel lastSelectedViewModel { get; set; } = null;

        public PhotographerWindow(MainWindowViewModel controller)
        {
            Controller = controller; 
            InitializeComponent();
            this.DataContext = Controller;
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (lastSelectedViewModel == null) return;
            if (DateTime.TryParse(Birthday.Text, out DateTime birthday))
            {
                if (birthday < DateTime.Now && !string.IsNullOrWhiteSpace(FirstName.Text) &&
                    !string.IsNullOrWhiteSpace(LastName.Text) && !string.IsNullOrWhiteSpace(Notes.Text))
                {

                    PhotographerViewModel photographerViewModel = lastSelectedViewModel;

                    photographerViewModel.FirstName = FirstName.Text;
                    photographerViewModel.LastName = LastName.Text;
                    photographerViewModel.BirthDay = DateTime.Parse(Birthday.Text);
                    photographerViewModel.Notes = Notes.Text;

                    Controller.UpdatePhotographer(photographerViewModel);
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

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lastSelectedViewModel == null) return;
            var photographerViewModel = lastSelectedViewModel;
            int ID = photographerViewModel.ID;
            try
            {
                Controller.DeletePhotographer(ID);
                FirstName.Text = string.Empty;
                LastName.Text = string.Empty;
                Birthday.Text = string.Empty;
                Notes.Text = string.Empty;
            }
            catch
            {
                MessageBox.Show("Can't delete this photographer because it its assigned to a picture.", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var PhotographerAddWindow = new PhotographerAddWindow(Controller);
            PhotographerAddWindow.Show();
        }

        private void PhotographerBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PhotographerBox.SelectedItem != null)
            {
                var PhotographerModel = (PhotographerViewModel)PhotographerBox.SelectedItem;
                lastSelectedViewModel = PhotographerModel;

                FirstName.Text = PhotographerModel.FirstName;
                LastName.Text = PhotographerModel.LastName;
                Birthday.Text = PhotographerModel.BirthDay.ToString();
                Notes.Text = PhotographerModel.Notes;
            }
            ErrorLabel.Content = "";
        }
    }
}
