using BuildingProjectManagement.Model;
using BuildingProjectManagement.Resources.Strings;
using BuildingProjectManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        static UploadDocumentWindow? uploadDocumentWindow;
        readonly UserViewModel userViewModel;
        readonly ProjectViewModel projectViewModel;
        readonly ContactViewModel contactViewModel;
        readonly DocumentViewModel documentViewModel;
        bool updateMode = false;
        private ICollectionView? ProjectsView;
        private bool isSelectionActive = false;

        public MainWindow(UserViewModel userViewModel)
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.WorkArea.Height + 14;
            this.MaxWidth = SystemParameters.WorkArea.Width + 14;
            this.userViewModel = userViewModel;
            this.contactViewModel = new ContactViewModel();
            this.projectViewModel = new ProjectViewModel();
            this.documentViewModel = new DocumentViewModel();
            contactViewModel.Contacts = new ObservableCollection<Contact>();
            projectViewModel.ProjectContacts = new ObservableCollection<Contact>();
            DataContext = projectViewModel;
            LabelTitle.Text = LabelTitle.Text + ActualSession.Session.LoggedInUser?.Name + 
                ActualSession.Session.LoggedInUser?.Surname;
            

            CbFilterStatus.Items.Add(AppStrings.StateAllProjects);

            foreach (var state in AppStrings.StateItems)
            {
                CbState.Items.Add(state);
                CbFilterStatus.Items.Add(state);
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await projectViewModel.ShowProjects();
            await contactViewModel.ShowContacts();
            CbContacts.DataContext = contactViewModel;
            CbFilterStatus.SelectedIndex = 0;
            ProjectsView = CollectionViewSource.GetDefaultView(projectViewModel.Projects);
            LbProjects.ItemsSource = ProjectsView;
            LbProjectDocs.DataContext = documentViewModel;
            LbPreviousDocs.DataContext = documentViewModel;
            LbExecutionDocs.DataContext = documentViewModel;
            LbFinalDocs.DataContext = documentViewModel;
            LbOtherDocs.DataContext = documentViewModel;
            LbOrders.DataContext = documentViewModel;
            LbIncidences.DataContext = documentViewModel;
            LabelOrderChecks.DataContext = documentViewModel;
            LabelIncidencesChecks.DataContext = documentViewModel;
        }

        // Métodos ventana general
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

        private void LbProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LbProjects.SelectedItem is not null)
            {
                TabControlProject.IsEnabled = true;
                projectViewModel.SelectedProject = (Project)LbProjects.SelectedItem;
                projectViewModel.ProjectContacts!.Clear();

                foreach (var contact in projectViewModel.SelectedProject.Contacts)
                {
                    projectViewModel.ProjectContacts.Add(contact);
                }

                SeeProjectData();

                int projectId = projectViewModel.SelectedProject.Id;
                ShowDocuments(projectId);
            }
            else
            {
                TabControlProject.IsEnabled = false;
            }
        }

        private void CbFilterStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProjectsView = CollectionViewSource.GetDefaultView(projectViewModel.Projects);

            if (CbFilterStatus.SelectedIndex > 0)
                ProjectsView.Filter = project => ((Project)project).State == CbFilterStatus.SelectedItem.ToString();
            else
                ProjectsView.Filter = null;

            ProjectsView.Refresh();
            LbProjects.ItemsSource = ProjectsView;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            foreach (string path in documentViewModel.TempDocuments!)
            {
                documentViewModel.DeleteTemporalDocument(path);
            }
        }

        // Métodos pestaña datos documento
        private void BtAddContact_Click(object sender, RoutedEventArgs e)
        {
            if (CbContacts.SelectedIndex > -1)
            {
                Contact selectedContact = (Contact)CbContacts.SelectedItem;
                bool contactInList = projectViewModel.ProjectContacts!.Any(contact => contact.Id == selectedContact.Id);

                if (!contactInList)
                {
                    projectViewModel.ProjectContacts!.Add((Contact)CbContacts.SelectedItem);
                    projectViewModel.CleanCheckMessage();
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

        private void SeeProjectData()
        {
            TbName.Text = projectViewModel.SelectedProject!.Name;
            TbSite.Text = projectViewModel.SelectedProject.Site;
            TbJobType.Text = projectViewModel.SelectedProject.JobType;
            TbDescription.Text = projectViewModel.SelectedProject.Description;
            LbContacts.ItemsSource = projectViewModel.SelectedProject.Contacts;
            CbState.Text = projectViewModel.SelectedProject.State.ToString();
        }

        private void ClearForm()
        {
            TbName.Clear();
            TbSite.Clear();
            TbJobType.Clear();
            TbDescription.Clear();
            projectViewModel.ProjectContacts!.Clear();
            CbContacts.SelectedIndex = 0;
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
            else
            {
                projectViewModel.ProjectChecksMessage = AppStrings.NoSelectedProject;
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
                        ClearForm();
                        LbContacts.ItemsSource = projectViewModel.ProjectContacts;
                    }
                    else
                    {
                        projectViewModel.ProjectChecksMessage = AppStrings.DeleteProjectError;
                    }
                }
            }
        }

        private async void BtSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            bool projectChecks = projectViewModel.UpdateProjectChecks(TbName.Text, TbSite.Text, TbJobType.Text);
            int selectedProjectIndex = LbProjects.SelectedIndex;

            if (projectChecks)
            {
                Project updatedProject = new Project(TbName.Text, TbSite.Text, TbJobType.Text, CbState.Text);
                updatedProject.Description = TbDescription.Text;

                List<Contact> contacts = projectViewModel.ProjectContacts!.ToList();

                updatedProject.Contacts = contacts;

                projectViewModel.ConfirmationTitle = AppStrings.ProjectUpdateTitle;
                projectViewModel.ConfirmationMessage = AppStrings.ProjectUpdateMessage;
                ProjectConfirmationWindow projectConfirmationWindow = new ProjectConfirmationWindow(projectViewModel);
                projectConfirmationWindow.ShowDialog();

                if (projectConfirmationWindow.DialogResult == true)
                {
                    projectViewModel.CleanCheckMessage();
                    var patchDoc = projectViewModel.GetPatchDoc(projectViewModel.SelectedProject!, updatedProject);
                    var response = await projectViewModel.PatchProject(projectViewModel.SelectedProject!.Id, projectViewModel.SelectedProject!, updatedProject);

                    if (response.IsSuccessStatusCode)
                    {
                        updateMode = false;
                        await projectViewModel.ShowProjects();
                        LbProjects.SelectedIndex = selectedProjectIndex;
                        EditContactsPanelVisibility(updateMode);
                        ChangeItemsVisibility();
                    }
                    else
                    {
                        projectViewModel.ProjectChecksMessage = AppStrings.UpdateProjectError;
                    }
                }
            }
        }

        private void BtCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            updateMode = false;

            EditContactsPanelVisibility(updateMode);
            ChangeItemsVisibility();
            SeeProjectData();
            projectViewModel.CleanCheckMessage();
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

        private void EditContactsPanelVisibility(bool visible)
        {
            if (visible)
            {
                EditContactsPanel.Visibility = Visibility.Visible;
                LbContacts.SetValue(Grid.RowSpanProperty, 1);
                BtUpdate.Visibility = Visibility.Collapsed;
                BtDeleteProject.Visibility = Visibility.Collapsed;
                BtSaveChanges.Visibility = Visibility.Visible;
                BtCancelChanges.Visibility = Visibility.Visible;
            }
            else
            {
                EditContactsPanel.Visibility = Visibility.Collapsed;
                LbContacts.SetValue(Grid.RowSpanProperty, 2);
                BtSaveChanges.Visibility = Visibility.Collapsed;
                BtCancelChanges.Visibility = Visibility.Collapsed;
                BtUpdate.Visibility = Visibility.Visible;
                BtDeleteProject.Visibility = Visibility.Visible;
            }
        }

        // Métodos pestaña documentos
        private async void ShowDocuments(int projectId)
        {
            var response = await documentViewModel.GetProjectDocumentsResponse(projectId);

            if (response.IsSuccessStatusCode)
            {
                var documents = await documentViewModel.GetProjectDocuments(response);
                documentViewModel.OrderDocuments(documents);
            }
        }

        private async void BtUploadDocument_Click(object sender, RoutedEventArgs e)
        {
            if (LbProjects.SelectedIndex > -1)
            {
                uploadDocumentWindow = new UploadDocumentWindow(documentViewModel);
                uploadDocumentWindow.ShowDialog();

                if (uploadDocumentWindow.DialogResult == true)
                {
                    var project = documentViewModel.DocumentToUpload!;
                    var response = await documentViewModel.PostDocument(project, projectViewModel.SelectedProject!.Id);

                    if (response.IsSuccessStatusCode)
                        ShowDocuments(projectViewModel.SelectedProject!.Id);
                    else
                        projectViewModel.ProjectDocumentMessage = AppStrings.UploadDocumentError;
                }
            }
        }

        private void BtOpenDocument_Click(object sender, RoutedEventArgs e)
        {
            if (documentViewModel.SelectedDocument is not null)
            {
                string url = documentViewModel.SelectedDocument.DocumentPath;
                try
                {
                    ProcessStartInfo process = new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    };

                    Process.Start(process);
                }
                catch (Exception)
                {
                    projectViewModel.ProjectDocumentMessage = AppStrings.OpenDocumentError;
                }
            }
            else
            {
                projectViewModel.ProjectDocumentMessage = AppStrings.NoSelectedDocumentError;
            }
        }

        private async void BtDeleteDocument_Click(object sender, RoutedEventArgs e)
        {
            if (documentViewModel.SelectedDocument is not null)
            {
                projectViewModel.ConfirmationTitle = AppStrings.DeleteDocumentConfirmationTitle;
                projectViewModel.ConfirmationMessage = AppStrings.DeleteDocumentConfirmationMessage;
                ProjectConfirmationWindow projectConfirmationWindow = new ProjectConfirmationWindow(projectViewModel);
                projectConfirmationWindow.ShowDialog();

                if (projectConfirmationWindow.DialogResult == true)
                {
                    int documentId = documentViewModel.SelectedDocument.Id;
                    int projectId = projectViewModel.SelectedProject!.Id;
                    var response = await documentViewModel.DeleteDocument(documentId, projectId);

                    if (response.IsSuccessStatusCode)
                    {
                        ShowDocuments(projectId);
                    }
                    else
                    {
                        projectViewModel.ProjectDocumentMessage = AppStrings.DeleteDocumentError;
                    }
                }
            }
            else
            {
                projectViewModel.ProjectDocumentMessage = AppStrings.NoSelectedDocumentError;
            }
        }

        private void LbProjectDocs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            projectViewModel.ProjectDocumentMessage = string.Empty;

            if (isSelectionActive)
                return;

            isSelectionActive = true;
            LbPreviousDocs.SelectedItem = null;
            LbExecutionDocs.SelectedItem = null;
            LbFinalDocs.SelectedItem = null;
            LbOtherDocs.SelectedItem = null;
            LbOrders.SelectedItem = null;
            LbIncidences.SelectedItem = null;
            documentViewModel.SelectedDocument = (ProjectDocument)LbProjectDocs.SelectedItem;
            isSelectionActive = false;
        }

        private void LbPreviousDocs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            projectViewModel.ProjectDocumentMessage = string.Empty;

            if (isSelectionActive)
                return;

            isSelectionActive = true;
            LbProjectDocs.SelectedItem = null;
            LbExecutionDocs.SelectedItem = null;
            LbFinalDocs.SelectedItem = null;
            LbOtherDocs.SelectedItem = null;
            LbOrders.SelectedItem = null;
            LbIncidences.SelectedItem = null;
            documentViewModel.SelectedDocument = (ProjectDocument)LbPreviousDocs.SelectedItem;
            isSelectionActive = false;
        }

        private void LbExecutionDocs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            projectViewModel.ProjectDocumentMessage = string.Empty;

            if (isSelectionActive)
                return;

            isSelectionActive = true;
            LbProjectDocs.SelectedItem = null;
            LbPreviousDocs.SelectedItem = null;
            LbFinalDocs.SelectedItem = null;
            LbOtherDocs.SelectedItem = null;
            LbOrders.SelectedItem = null;
            LbIncidences.SelectedItem = null;
            documentViewModel.SelectedDocument = (ProjectDocument)LbExecutionDocs.SelectedItem;
            isSelectionActive = false;
        }

        private void LbFinalDocs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            projectViewModel.ProjectDocumentMessage = string.Empty;

            if (isSelectionActive)
                return;

            isSelectionActive = true;
            LbProjectDocs.SelectedItem = null;
            LbPreviousDocs.SelectedItem = null;
            LbExecutionDocs.SelectedItem = null;
            LbOtherDocs.SelectedItem = null;
            LbOrders.SelectedItem = null;
            LbIncidences.SelectedItem = null;
            documentViewModel.SelectedDocument = (ProjectDocument)LbFinalDocs.SelectedItem;
            isSelectionActive = false;
        }

        private void LbOtherDocs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            projectViewModel.ProjectDocumentMessage = string.Empty;

            if (isSelectionActive)
                return;

            isSelectionActive = true;
            LbProjectDocs.SelectedItem = null;
            LbPreviousDocs.SelectedItem = null;
            LbExecutionDocs.SelectedItem = null;
            LbFinalDocs.SelectedItem = null;
            LbOrders.SelectedItem = null;
            LbIncidences.SelectedItem = null;
            documentViewModel.SelectedDocument = (ProjectDocument)LbOtherDocs.SelectedItem;
            isSelectionActive = false;
        }

        private void LbOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            documentViewModel.DocumentChecksMessage = string.Empty;

            if (isSelectionActive)
                return;

            isSelectionActive = true;
            LbProjectDocs.SelectedItem = null;
            LbPreviousDocs.SelectedItem = null;
            LbExecutionDocs.SelectedItem = null;
            LbFinalDocs.SelectedItem = null;
            LbOtherDocs.SelectedItem = null;
            LbIncidences.SelectedItem = null;
            documentViewModel.SelectedDocument = (ProjectDocument)LbOrders.SelectedItem;
            isSelectionActive = false;
        }

        private void LbIncidences_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            documentViewModel.DocumentChecksMessage = string.Empty;

            if (isSelectionActive)
                return;

            isSelectionActive = true;
            LbProjectDocs.SelectedItem = null;
            LbPreviousDocs.SelectedItem = null;
            LbExecutionDocs.SelectedItem = null;
            LbFinalDocs.SelectedItem = null;
            LbOtherDocs.SelectedItem = null;
            LbOrders.SelectedItem = null;
            documentViewModel.SelectedDocument = (ProjectDocument)LbIncidences.SelectedItem;
            isSelectionActive = false;
        }

        // Métodos pestaña órdenes
        private async void BtCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            Contact contact = (Contact)CbOrderContact.SelectedItem;
            string contactName = string.Empty;

            if (contact is not null)
                contactName = contact.Name;

            bool checkOrder = documentViewModel.CheckOrder(contactName, TbOrderTitle.Text, TbOrderContent.Text);

            if (checkOrder)
            {
                DocumentPost? document = documentViewModel.CreateDocumentPdf(TbOrderTitle.Text, contactName, TbOrderContent.Text, 
                    AppStrings.OrderCategory);
                int projectId = projectViewModel.SelectedProject!.Id;

                if (document is not null)
                {
                    var response = await documentViewModel.PostDocument(document, projectId);

                    if (response.IsSuccessStatusCode)
                    {
                        ShowDocuments(projectId);
                    }
                    else
                    {
                        documentViewModel.DocumentChecksMessage = AppStrings.GenerateOrderError;
                    }
                }
            }
        }

        private void BtOpenOrder_Click(object sender, RoutedEventArgs e)
        {
            if (LbOrders.SelectedIndex > -1)
            {
                string url = documentViewModel.SelectedDocument!.DocumentPath;
                try
                {
                    ProcessStartInfo process = new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    };

                    Process.Start(process);
                }
                catch (Exception)
                {
                    documentViewModel.DocumentChecksMessage = AppStrings.OpenDocumentError;
                }
            }
            else
            {
                documentViewModel.DocumentChecksMessage = AppStrings.NoSelectedDocumentError;
            }
        }

        private async void BtDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (LbOrders.SelectedIndex > -1)
            {
                projectViewModel.ConfirmationTitle = AppStrings.DeleteDocumentConfirmationTitle;
                projectViewModel.ConfirmationMessage = AppStrings.DeleteDocumentConfirmationMessage;
                ProjectConfirmationWindow projectConfirmationWindow = new ProjectConfirmationWindow(projectViewModel);
                projectConfirmationWindow.ShowDialog();

                if (projectConfirmationWindow.DialogResult == true)
                {
                    int documentId = documentViewModel.SelectedDocument!.Id;
                    int projectId = projectViewModel.SelectedProject!.Id;
                    var response = await documentViewModel.DeleteDocument(documentId, projectId);

                    if (response.IsSuccessStatusCode)
                    {
                        ShowDocuments(projectId);
                    }
                    else
                    {
                        documentViewModel.DocumentChecksMessage = AppStrings.DeleteDocumentError;
                    }
                }
            }
            else
            {
                documentViewModel.DocumentChecksMessage = AppStrings.NoSelectedDocumentError;
            }
        }

        // Métodos pestaña incidencias
        private async void BtCreateIncidence_Click(object sender, RoutedEventArgs e)
        {
            Contact contact = (Contact)CbIncidencesContact.SelectedItem;
            string contactName = string.Empty;

            if (contact is not null)
                contactName = contact.Name;

            bool checkOrder = documentViewModel.CheckOrder(contactName, TbIncidencesTitle.Text, TbIncidencesContent.Text);

            if (checkOrder)
            {
                DocumentPost? document = documentViewModel.CreateDocumentPdf(TbIncidencesTitle.Text, contactName, TbIncidencesContent.Text, 
                    AppStrings.IncidenceCategory);
                int projectId = projectViewModel.SelectedProject!.Id;

                if (document is not null)
                {
                    var response = await documentViewModel.PostDocument(document, projectId);

                    if (response.IsSuccessStatusCode)
                    {
                        ShowDocuments(projectId);
                    }
                    else
                    {
                        documentViewModel.DocumentChecksMessage = AppStrings.GenerateOrderError;
                    }
                }
            }
        }

        private void BtOpenIncidence_Click(object sender, RoutedEventArgs e)
        {
            if (LbIncidences.SelectedIndex > -1)
            {
                string url = documentViewModel.SelectedDocument!.DocumentPath;
                try
                {
                    ProcessStartInfo process = new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    };

                    Process.Start(process);
                }
                catch (Exception)
                {
                    documentViewModel.DocumentChecksMessage = AppStrings.OpenDocumentError;
                }
            }
            else
            {
                documentViewModel.DocumentChecksMessage = AppStrings.NoSelectedDocumentError;
            }
        }

        private async void BtDeleteIncidence_Click(object sender, RoutedEventArgs e)
        {
            if (LbIncidences.SelectedIndex > -1)
            {
                projectViewModel.ConfirmationTitle = AppStrings.DeleteDocumentConfirmationTitle;
                projectViewModel.ConfirmationMessage = AppStrings.DeleteDocumentConfirmationMessage;
                ProjectConfirmationWindow projectConfirmationWindow = new ProjectConfirmationWindow(projectViewModel);
                projectConfirmationWindow.ShowDialog();

                if (projectConfirmationWindow.DialogResult == true)
                {
                    int documentId = documentViewModel.SelectedDocument!.Id;
                    int projectId = projectViewModel.SelectedProject!.Id;
                    var response = await documentViewModel.DeleteDocument(documentId, projectId);

                    if (response.IsSuccessStatusCode)
                    {
                        ShowDocuments(projectId);
                    }
                    else
                    {
                        documentViewModel.DocumentChecksMessage = AppStrings.DeleteDocumentError;
                    }
                }
            }
            else
            {
                documentViewModel.DocumentChecksMessage = AppStrings.NoSelectedDocumentError;
            }
        }
    }
}
