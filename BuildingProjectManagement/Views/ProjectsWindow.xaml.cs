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
    public partial class ProjectsWindow : Window
    {
        ContactViewModel contactViewModel;
        ProjectViewModel projectViewModel;

        public ProjectsWindow(ContactViewModel contactViewModel, ProjectViewModel projectViewModel)
        {
            InitializeComponent();
            this.contactViewModel = contactViewModel;
            this.projectViewModel = projectViewModel;
            DataContext = contactViewModel;
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            checkMessage.DataContext = projectViewModel;
            await contactViewModel.ShowContacts();
        }

        private void btMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            projectViewModel.ProjectChecksMessage = "Prueba de contenido del mensaje";
        }

        private void BtDelete_Click(object sender, RoutedEventArgs e)
        {
            projectViewModel.ProjectChecksMessage = "Otra prueba de contenido del mensaje";
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtClear_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
