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
    public partial class ProjectsWindow : Window
    {
        ContactViewModel contactViewModel;
        ProjectViewModel projectViewModel;
        List<int> userContacts;

        public ProjectsWindow(ContactViewModel contactViewModel, ProjectViewModel projectViewModel)
        {
            InitializeComponent();
            this.contactViewModel = contactViewModel;
            this.projectViewModel = projectViewModel;
            DataContext = contactViewModel;
            userContacts = new List<int>();
            
            foreach (var state in AppStrings.StateItems)
            {
                CbState.Items.Add(state);
            }

            CbState.SelectedIndex = 0;
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
            DialogResult = false;
            Close();
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (CbContacts.SelectedIndex > -1)
            {
                if (!userContacts.Contains(((Contact)CbContacts.SelectedItem).Id))
                {
                    userContacts.Add(((Contact)CbContacts.SelectedItem).Id);
                    LbContacts.Items.Add((Contact)CbContacts.SelectedItem);
                }
                else
                {
                    projectViewModel.ProjectChecksMessage = AppStrings.ContactInListError;
                }
            }
            else
            {
                projectViewModel.ProjectChecksMessage = AppStrings.NoSelectedContactError;
            }
        }

        private void BtDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LbContacts.Items.Count > 0)
            {
                if (LbContacts.SelectedItem is not null)
                {
                    userContacts.Remove(((Contact)LbContacts.SelectedItem).Id);
                    LbContacts.Items.Remove((Contact)LbContacts.SelectedItem);
                }
                else
                {
                    projectViewModel.ProjectChecksMessage = AppStrings.NoSelectedContactError;
                }
            }
            else
            {
                projectViewModel.ProjectChecksMessage = AppStrings.EmptyListError;
            }
        }

        private async void BtSave_Click(object sender, RoutedEventArgs e)
        {
            bool projectChecks = projectViewModel.ProjectChecks(TbName.Text, TbSite.Text, TbJobType.Text);
            
            if (projectChecks)
            {
                if (CbState.SelectedIndex > -1)
                { 
                    Project project = new Project(TbName.Text, TbSite.Text, TbJobType.Text, CbState.SelectedItem.ToString()!);
                    projectViewModel.ValidateProjectForm(project, TbDescription.Text);
                    project.ContactsIds = userContacts;
                    var response = await projectViewModel.PostProject(project);

                    if (response.IsSuccessStatusCode)
                    {
                        ClearForm();
                        DialogResult = true;
                        Close();
                    }
                    else
                    {
                        projectViewModel.CheckMessage = AppStrings.ProjectCreationError;
                    }
                }
                else
                {
                    projectViewModel.CheckMessage = AppStrings.CheckState;
                }   
            }
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
            projectViewModel.CleanCheckMessage();
        }

        private void BtClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            projectViewModel.CleanCheckMessage();
        }

        private void ClearForm()
        {
            TbName.Clear();
            TbSite.Clear();
            TbJobType.Clear();
            TbDescription.Clear();
            CbContacts.SelectedItem = null;
            LbContacts.Items.Clear();
            userContacts.Clear();
            CbState.SelectedIndex = 0;
            projectViewModel.CleanCheckMessage();
        }
    }
}
