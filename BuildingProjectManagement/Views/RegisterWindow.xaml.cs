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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BuildingProjectManagement.Views
{
    public partial class RegisterWindow : Window
    {
        private bool passwordVisible = false;
        static LoginWindow? loginWindow;
        static RegisterConfirmationWindow? registerConfirmationWindow;
        UserViewModel userViewModel;

        public RegisterWindow()
        {
            InitializeComponent();
            userViewModel = new UserViewModel();
            DataContext = userViewModel;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }

        private async void btAccept_Click(object sender, RoutedEventArgs e)
        {
            bool checkData = userViewModel.CheckUserData(tbName.Text, tbSurname.Text, tbDni.Text,
                tbEmail.Text, pbPassword.Password, pbRepPassword.Password);

            if (checkData)
            {
                string name = userViewModel.ChangeFirstChar(tbName.Text);
                string surname = userViewModel.ChangeFirstChar(tbSurname.Text);

                var userCredentials = new UserCredentialsDTO(tbEmail.Text, pbPassword.Password);
                var userRegister = new UserRegisterDTO(name, surname, tbDni.Text, tbEmail.Text);

                var user = new User(userCredentials, userRegister);

                await userViewModel.Register(user);

                registerConfirmationWindow = new RegisterConfirmationWindow(userViewModel);
                registerConfirmationWindow.ShowDialog();
                loginWindow = new LoginWindow();
                loginWindow.Show();

                Close();
            }
        }

        private void btVisibility_Click(object sender, RoutedEventArgs e)
        {
            passwordVisible = !passwordVisible;

            if (passwordVisible)
            {
                tbPassword.Visibility = Visibility.Visible;
                pbPassword.Visibility = Visibility.Collapsed;

                tbRepPassword.Visibility = Visibility.Visible;
                pbRepPassword.Visibility = Visibility.Collapsed;

                iconVisibility.Source = new BitmapImage(new Uri("/Resources/Images/hidden.png", UriKind.Relative));
            }
            else
            {
                pbPassword.Visibility = Visibility.Visible;
                tbPassword.Visibility = Visibility.Collapsed;

                pbRepPassword.Visibility = Visibility.Visible;
                tbRepPassword.Visibility = Visibility.Collapsed;

                iconVisibility.Source = new BitmapImage(new Uri("/Resources/Images/eye.png", UriKind.Relative));
            }
        }

        private void pbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pbPassword.Password != tbPassword.Text)
            {
                tbPassword.Text = pbPassword.Password;
            }
        }

        private void tbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (pbPassword.Password != tbPassword.Text)
            {
                pbPassword.Password = tbPassword.Text;
            }
        }

        private void pbRepPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pbRepPassword.Password != tbRepPassword.Text)
            {
                tbRepPassword.Text = pbRepPassword.Password;
            }
        }

        private void tbRepPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (pbRepPassword.Password != tbRepPassword.Text)
            {
                pbRepPassword.Password = tbRepPassword.Text;
            }
        }
    }
}
