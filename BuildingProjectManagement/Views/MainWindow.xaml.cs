using BuildingProjectManagement.Model;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BuildingProjectManagement.Views
{
    public partial class MainWindow : Window
    {
        static ContactsWindow? contactsWindow;
        UserViewModel userViewModel;
        ContactViewModel contactViewModel;

        public MainWindow(UserViewModel userViewModel)
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.WorkArea.Height + 14;
            this.MaxWidth = SystemParameters.WorkArea.Width + 14;
            this.userViewModel = userViewModel;
            this.contactViewModel = new ContactViewModel();
            DataContext = userViewModel;
            LabelTitle.Text = LabelTitle.Text + ActualSession.Session.LoggedInUser?.Name + 
                ActualSession.Session.LoggedInUser?.Surname;
        }

        private void BtMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                BtMaximize.Content = "\uE922";
            }
            else
            {
                WindowState = WindowState.Maximized;
                BtMaximize.Content = "\uE923";
            }
        }

        private void BtClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtContacts_Click(object sender, RoutedEventArgs e)
        {
            contactsWindow = new ContactsWindow(contactViewModel);
            contactsWindow.ShowDialog();
        }

        private void BtCloseSession_Click(object sender, RoutedEventArgs e)
        {
            CloseSessionConfirmation closeSessionConfirmation = new CloseSessionConfirmation();
            bool? result = closeSessionConfirmation.ShowDialog();

            if (result == true)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                Close();
            }
        }

        private void BtProject_Click(object sender, RoutedEventArgs e)
        {
            ProjectsWindow projectsWindow = new ProjectsWindow();
            projectsWindow.Show();
        }
    }
}
