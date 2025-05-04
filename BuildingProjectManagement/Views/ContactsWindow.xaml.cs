using BuildingProjectManagement.Model;
using BuildingProjectManagement.Resources.Strings;
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
        ContactViewModel contactViewModel;
        private bool deleteMode;

        public ContactsWindow(ContactViewModel contactViewModel)
        {
            InitializeComponent();
            this.contactViewModel = contactViewModel;
            ListContacts.ItemsSource = contactViewModel.Contacts;
            DataContext = this.contactViewModel;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await contactViewModel.ShowContacts();
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

        private async void BtSave_Click(object sender, RoutedEventArgs e)
        {
            bool checks = contactViewModel.ContactChecks(TbName.Text, TbDni.Text, TbPhone.Text, TbEmail.Text, TbProfession.Text);

            if (checks)
            {
                string name = contactViewModel.ChangeFirstChar(TbName.Text);

                if (ListContacts.SelectedItem == null)
                {
                    Contact contact = new Contact(name, TbDni.Text, TbProfession.Text);
                    contactViewModel.ValidateContactForm(contact, TbAddress.Text, TbTown.Text, TbProvince.Text, TbPhone.Text, TbEmail.Text);
                    var response = await contactViewModel.PostContact(contact);

                    if (response.IsSuccessStatusCode)
                    {
                        ClearForm();
                        await contactViewModel.ShowContacts();
                    }
                    else
                    {
                        contactViewModel.CheckMessage = AppStrings.ContactCreateError;
                    }
                }
                else
                {
                    deleteMode = false;
                    contactViewModel.SelectedContact!.Name = name;
                    contactViewModel.ValidateContactForm(contactViewModel.SelectedContact, TbAddress.Text, TbTown.Text, TbProvince.Text, 
                        TbPhone.Text, TbEmail.Text);
                    ConfirmationWindow confirmationWindow = new ConfirmationWindow(contactViewModel, contactViewModel.SelectedContact, deleteMode);
                    contactViewModel.ConfirmationWindowTitle = AppStrings.ConfirmationWindowUpdateTitle;
                    contactViewModel.ConfirmationWindowValidation = AppStrings.ConfirmationWindowUpdateMessage;
                    confirmationWindow.ShowDialog();
                    await contactViewModel.ShowContacts();
                    ClearForm();
                }
            }
        }

        private async void BtDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ListContacts.SelectedItem == null)
            {
                contactViewModel.CheckMessage = AppStrings.ContactDeleteErrorSelection;
            }
            else
            {
                deleteMode = true;
                ConfirmationWindow confirmationWindow = new ConfirmationWindow(contactViewModel, contactViewModel.SelectedContact!, deleteMode);
                contactViewModel.ConfirmationWindowTitle = AppStrings.ConfirmationWindowDeleteTitle;
                contactViewModel.ConfirmationWindowValidation = AppStrings.ConfirmationWindowDeleteMessage;
                confirmationWindow.ShowDialog();
                await contactViewModel.ShowContacts();
                ClearForm();
            }   
        }

        private void BtClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            TbName.Clear();
            TbDni.Clear();
            TbAddress.Clear();
            TbTown.Clear();
            TbProvince.Clear();
            TbPhone.Clear();
            TbEmail.Clear();
            TbProfession.Clear();
            ListContacts.SelectedItem = null;
            contactViewModel.CleanCheckMessage();
        }

        private void ListContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListContacts.SelectedIndex > -1)
            {
                contactViewModel.SelectedContact = (Contact)ListContacts.SelectedItem;
                TbName.Text = contactViewModel.SelectedContact.Name;
                TbDni.Text = contactViewModel.SelectedContact.Dni;
                TbAddress.Text = contactViewModel.SelectedContact.Address;
                TbTown.Text = contactViewModel.SelectedContact.Town;
                TbProvince.Text = contactViewModel.SelectedContact.Province;
                TbPhone.Text = contactViewModel.SelectedContact.Phone;
                TbEmail.Text = contactViewModel.SelectedContact.Email;
                TbProfession.Text = contactViewModel.SelectedContact.Profession;
                contactViewModel.CleanCheckMessage();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            contactViewModel.CleanCheckMessage();
        }
    }
}
