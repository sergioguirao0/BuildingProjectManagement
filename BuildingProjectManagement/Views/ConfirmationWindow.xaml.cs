using BuildingProjectManagement.Model;
using BuildingProjectManagement.Resources.Strings;
using BuildingProjectManagement.ViewModel;
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

namespace BuildingProjectManagement.Views
{
    public partial class ConfirmationWindow : Window
    {
        readonly ContactViewModel contactViewModel;
        Contact contact;
        private bool deleteMode;

        public ConfirmationWindow(ContactViewModel contactViewModel, Contact contact, bool deleteMode)
        {
            InitializeComponent();
            this.contactViewModel = contactViewModel;
            this.contact = contact;
            this.deleteMode = deleteMode;
            LabelName.Text = contact.Name;
            DataContext = contactViewModel;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void BtAccept_Click(object sender, RoutedEventArgs e)
        {
            int id = contact.Id;

            if (deleteMode)
            {
                var response = await contactViewModel.DeleteContact(id);

                if (!response.IsSuccessStatusCode)
                    contactViewModel.CheckMessage = AppStrings.ContactDeleteError;

                Close();
            }
            else
            {
                var response = await contactViewModel.PutContact(id, contact);

                if (!response.IsSuccessStatusCode)
                    contactViewModel.CheckMessage = AppStrings.ContactUpdateError;

                Close();
            }
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
