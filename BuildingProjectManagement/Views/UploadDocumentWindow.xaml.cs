using BuildingProjectManagement.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Win32;
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
using System.IO;
using Path = System.IO.Path;
using BuildingProjectManagement.Resources.Strings;

namespace BuildingProjectManagement.Views
{
    public partial class UploadDocumentWindow : Window
    {
        readonly DocumentViewModel documentViewModel;
        string? filePath;

        public UploadDocumentWindow(DocumentViewModel documentViewModel)
        {
            InitializeComponent();
            DataContext = documentViewModel;
            this.documentViewModel = documentViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CbCategory.ItemsSource = AppStrings.CategoryItems;
        }

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtAccept_Click(object sender, RoutedEventArgs e)
        {
            bool isValidDocument = documentViewModel.CheckDocument(TbTitle.Text, CbCategory.Text, filePath!); 

            if (isValidDocument)
            {
                documentViewModel.DocumentToUpload!.Title = TbTitle.Text;
                documentViewModel.DocumentToUpload!.Category = CbCategory.Text;
                documentViewModel.DocumentToUpload!.DocumentPath = filePath!;
                DialogResult = true;
                Close();
            }
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtSearchDoc_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = AppStrings.PdfFilter;

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                TbDocument.Text = Path.GetFileName(filePath);
            }
        }

        
    }
}
