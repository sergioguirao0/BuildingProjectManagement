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
    public partial class ProjectConfirmationWindow : Window
    {
        readonly ProjectViewModel projectViewModel;

        public ProjectConfirmationWindow(ProjectViewModel projectViewModel)
        {
            InitializeComponent();
            this.projectViewModel = projectViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = projectViewModel;
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
            DialogResult = false;
            Close();
        }

        private void BtAccept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
