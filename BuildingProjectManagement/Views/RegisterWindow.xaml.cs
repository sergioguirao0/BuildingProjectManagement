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

        public RegisterWindow()
        {
            InitializeComponent();
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
            this.Close();
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void btAccept_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btVisibility_Click(object sender, RoutedEventArgs e)
        {
            passwordVisible = !passwordVisible;

            if (passwordVisible)
            {
                tbPassword.Text = pbPassword.Password;
                tbPassword.Visibility = Visibility.Visible;
                pbPassword.Visibility = Visibility.Collapsed;

                tbRepPassword.Text = pbRepPassword.Password;
                tbRepPassword.Visibility = Visibility.Visible;
                pbRepPassword.Visibility = Visibility.Collapsed;

                iconVisibility.Source = new BitmapImage(new Uri("/Resources/Images/hidden.png", UriKind.Relative));
            }
            else
            {
                pbPassword.Password = tbPassword.Text;
                pbPassword.Visibility = Visibility.Visible;
                tbPassword.Visibility = Visibility.Collapsed;

                pbRepPassword.Password = tbRepPassword.Text;
                pbRepPassword.Visibility = Visibility.Visible;
                tbRepPassword.Visibility = Visibility.Collapsed;

                iconVisibility.Source = new BitmapImage(new Uri("/Resources/Images/eye.png", UriKind.Relative));
            }
        }
    }
}
