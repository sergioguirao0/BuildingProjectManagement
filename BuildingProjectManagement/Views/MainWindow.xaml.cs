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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BuildingProjectManagement.Views
{
    public partial class MainWindow : Window
    {
        static ProjectsWindow? projectsWindow;
        static ContactsWindow? contactsWindow;
        readonly UserViewModel userViewModel;
        readonly ProjectViewModel projectViewModel;
        readonly ContactViewModel contactViewModel;
        bool updateMode = false;

        public MainWindow(UserViewModel userViewModel)
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.WorkArea.Height + 14;
            this.MaxWidth = SystemParameters.WorkArea.Width + 14;
            this.userViewModel = userViewModel;
            this.contactViewModel = new ContactViewModel();
            this.projectViewModel = new ProjectViewModel();
            contactViewModel.Contacts = new ObservableCollection<Contact>();
            projectViewModel.ProjectContacts = new ObservableCollection<Contact>();
            DataContext = projectViewModel;
            LabelTitle.Text = LabelTitle.Text + ActualSession.Session.LoggedInUser?.Name + 
                ActualSession.Session.LoggedInUser?.Surname;

            foreach (var state in AppStrings.StateItems)
            {
                CbState.Items.Add(state);
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await projectViewModel.ShowProjects();
            await contactViewModel.ShowContacts();
            CbContacts.DataContext = contactViewModel;
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

        private async void BtProject_Click(object sender, RoutedEventArgs e)
        {
            projectsWindow = new ProjectsWindow(contactViewModel, projectViewModel);
            projectsWindow.ShowDialog();

            if (projectsWindow.DialogResult == true)
            {
                await projectViewModel.ShowProjects();
            }
        }

        private void BtAddContact_Click(object sender, RoutedEventArgs e)
        {
            if (CbContacts.SelectedIndex > -1)
            {
                if (!projectViewModel.ProjectContacts!.Contains((Contact)CbContacts.SelectedItem))
                {
                    projectViewModel.ProjectContacts.Add((Contact)CbContacts.SelectedItem);
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

        private void BtDeleteContact_Click(object sender, RoutedEventArgs e)
        {
            if (LbContacts.Items.Count > 0)
            {
                if (LbContacts.SelectedItem is not null)
                {
                    projectViewModel.ProjectContacts!.Remove((Contact)LbContacts.SelectedItem);
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

        private void LbProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LbProjects.SelectedItem is not null)
            {
                projectViewModel.SelectedProject = (Project)LbProjects.SelectedItem;
                projectViewModel.ProjectContacts!.Clear();

                foreach (var contact in projectViewModel.SelectedProject.Contacts)
                {
                    projectViewModel.ProjectContacts.Add(contact);
                }

                SeeProjectData();
            }
        }

        private void SeeProjectData()
        {
            TbName.Text = projectViewModel.SelectedProject!.Name;
            TbSite.Text = projectViewModel.SelectedProject.Site;
            TbJobType.Text = projectViewModel.SelectedProject.JobType;
            TbDescription.Text = projectViewModel.SelectedProject.Description;
            LbContacts.ItemsSource = projectViewModel.SelectedProject.Contacts;
            CbState.Text = projectViewModel.SelectedProject.State.ToString();
        }

        private void BtUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (LbProjects.SelectedItem is not null)
            {
                updateMode = true;
                projectViewModel.CleanCheckMessage();
                EditContactsPanelVisibility(updateMode);
                ChangeItemsVisibility();
                BtUpdate.Visibility = Visibility.Collapsed;
                BtDeleteProject.Visibility = Visibility.Collapsed;
                BtSaveChanges.Visibility = Visibility.Visible;
                BtCancelChanges.Visibility = Visibility.Visible;
                LbContacts.ItemsSource = projectViewModel.ProjectContacts;
            }
        }

        private async void BtDeleteProject_Click(object sender, RoutedEventArgs e)
        {
            if (LbProjects.SelectedItem is null)
            {
                projectViewModel.ProjectChecksMessage = AppStrings.NoSelectedProject;
            }
            else
            {
                Project project = (Project)LbProjects.SelectedItem;
                projectViewModel.ConfirmationTitle = AppStrings.ProjectDeleteTitle;
                projectViewModel.ConfirmationMessage = AppStrings.ProjectDeleteMessage;
                ProjectConfirmationWindow projectConfirmationWindow = new ProjectConfirmationWindow(projectViewModel);
                projectConfirmationWindow.ShowDialog();

                if (projectConfirmationWindow.DialogResult == true)
                {
                    var response = await projectViewModel.DeleteProject(project.Id);

                    if (response.IsSuccessStatusCode)
                    {
                        await projectViewModel.ShowProjects();
                        LbProjects.SelectedItem = null;
                    }
                    else
                    {
                        projectViewModel.ProjectChecksMessage = AppStrings.DeleteProjectError;
                    }
                }
            }
        }

        private void BtSaveChanges_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            updateMode = false;

            EditContactsPanelVisibility(updateMode);
            ChangeItemsVisibility();
            BtUpdate.Visibility = Visibility.Visible;
            BtDeleteProject.Visibility = Visibility.Visible;
            BtSaveChanges.Visibility = Visibility.Collapsed;
            BtCancelChanges.Visibility = Visibility.Collapsed;
            SeeProjectData();
            projectViewModel.CleanCheckMessage();
        }

        private void EditContactsPanelVisibility(bool visible)
        {
            if (visible)
            {
                EditContactsPanel.Visibility = Visibility.Visible;
                LbContacts.SetValue(Grid.RowSpanProperty, 1);
            }
            else
            {
                EditContactsPanel.Visibility = Visibility.Collapsed;
                LbContacts.SetValue(Grid.RowSpanProperty, 2);
            }
        }

        private void ChangeItemsVisibility()
        {
            TbName.IsReadOnly = !TbName.IsReadOnly;
            TbSite.IsReadOnly = !TbSite.IsReadOnly;
            TbJobType.IsReadOnly = !TbJobType.IsReadOnly;
            TbDescription.IsReadOnly = !TbDescription.IsReadOnly;
            CbState.IsEnabled = !CbState.IsEnabled;
            LbProjects.IsEnabled = !LbProjects.IsEnabled;
        }
    }
}
