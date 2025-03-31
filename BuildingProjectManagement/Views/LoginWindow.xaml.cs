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
        private static bool _passwordVisible = false;
        private static RegisterWindow? _registerWindow;

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
            _passwordVisible = !_passwordVisible;

            if (_passwordVisible)
            {
                TbPassword.Text = PbPassword.Password;
                TbPassword.Visibility = Visibility.Visible;
                PbPassword.Visibility = Visibility.Collapsed;
                IconVisibility.Source = new BitmapImage(new Uri("/Resources/Images/hidden.png", UriKind.Relative));
            }
            else
            {
                PbPassword.Password = TbPassword.Text;
                PbPassword.Visibility = Visibility.Visible;
                TbPassword.Visibility = Visibility.Collapsed;
                IconVisibility.Source = new BitmapImage(new Uri("/Resources/Images/eye.png", UriKind.Relative));
            }
        }

        private void btRegister_Click(object sender, RoutedEventArgs e)
        {
            _registerWindow = new RegisterWindow();
            _registerWindow.Show();
            this.Close();
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
