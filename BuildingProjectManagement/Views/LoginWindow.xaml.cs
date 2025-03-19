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
        private bool passwordVisible = false;
        static RegisterWindow? registerWindow;

        public LoginWindow()
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
            Close();
        }

        private void btVisibility_Click(object sender, RoutedEventArgs e)
        {
            passwordVisible = !passwordVisible;

            if (passwordVisible)
            {
                tbPassword.Text = pbPassword.Password;
                tbPassword.Visibility = Visibility.Visible;
                pbPassword.Visibility = Visibility.Collapsed;
                iconVisibility.Source = new BitmapImage(new Uri("/Resources/Images/hidden.png", UriKind.Relative));
            }
            else
            {
                pbPassword.Password = tbPassword.Text;
                pbPassword.Visibility = Visibility.Visible;
                tbPassword.Visibility = Visibility.Collapsed;
                iconVisibility.Source = new BitmapImage(new Uri("/Resources/Images/eye.png", UriKind.Relative));
            }
        }

        private void btRegister_Click(object sender, RoutedEventArgs e)
        {
            registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
