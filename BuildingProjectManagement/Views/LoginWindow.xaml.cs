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
    public partial class LoginWindow : Window
    {
        UserViewModel userViewModel;
        private static bool _passwordVisible = false;
        static RegisterWindow? registerWindow;
        static MainWindow? mainWindow;

        public LoginWindow()
        {
            InitializeComponent();
            this.userViewModel = new UserViewModel();
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
            Close();
        }

        private void btVisibility_Click(object sender, RoutedEventArgs e)
        {
            _passwordVisible = !_passwordVisible;

            if (_passwordVisible)
            {
                TbPassword.Visibility = Visibility.Visible;
                PbPassword.Visibility = Visibility.Collapsed;
                IconVisibility.Source = new BitmapImage(new Uri("/Resources/Images/hidden.png", UriKind.Relative));
            }
            else
            {
                PbPassword.Visibility = Visibility.Visible;
                TbPassword.Visibility = Visibility.Collapsed;
                IconVisibility.Source = new BitmapImage(new Uri("/Resources/Images/eye.png", UriKind.Relative));
            }
        }

        private void TbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TbPassword.Text != PbPassword.Password)
            {
                PbPassword.Password = TbPassword.Text;
            }
        }

        private void PbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PbPassword.Password != TbPassword.Text)
            {
                TbPassword.Text = PbPassword.Password;
            }
        }

        private void btRegister_Click(object sender, RoutedEventArgs e)
        {
            registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }

        private async void btLogin_Click(object sender, RoutedEventArgs e)
        {
            var userLogin = new UserCredentialsDTO(TbUser.Text, PbPassword.Password);
            var response = await userViewModel.LoginUser(userLogin);

            if (response.IsSuccessStatusCode)
            {
                var token = await userViewModel.GetToken(response);
                var sessionResponse = await userViewModel.GetLoggedInUser(token);

                if (sessionResponse.IsSuccessStatusCode)
                {
                    var user = await userViewModel.GetUser(sessionResponse);

                    ActualSession.Session.Token = token;
                    ActualSession.Session.LoggedInUser = user;

                    mainWindow = new MainWindow(userViewModel);
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    ErrorTextLabel.Text = AppStrings.LoginError;
                }
            }
            else
            {
                ErrorTextLabel.Text = AppStrings.LoginError;
            }
        }
    }
}
