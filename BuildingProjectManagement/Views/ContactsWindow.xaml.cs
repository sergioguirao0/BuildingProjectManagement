using BuildingProjectManagement.Model;
using BuildingProjectManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class ContactsWindow : Window
    {
        public ObservableCollection<Contact> Contacts { get; set; }
        ContactViewModel contactViewModel;

        public ContactsWindow()
        {
            InitializeComponent();
            contactViewModel = new ContactViewModel();
            Contacts = new ObservableCollection<Contact>();
            ListContacts.ItemsSource = Contacts;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await ShowContacts();
        }

        private async Task ShowContacts()
        {
            var response = await contactViewModel.GetContactResponse(ActualSession.Session.Token!);

            if (response.IsSuccessStatusCode)
            {
                var contacts = await contactViewModel.GetContacts(response);
                Contacts.Clear();

                foreach(var contact in contacts!)
                {
                    Contacts.Add(contact);
                }
            }
        }

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtClear_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
