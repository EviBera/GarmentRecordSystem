using GarmentRecordSystem.Models;
using GarmentRecordSystem.Services;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;


namespace WPFbasedUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IGarmentService _garmentService;
        private readonly IJsonHandlerService _jsonHandlerService;
        private bool _unsavedChanges = false;


        public MainWindow()
        {
            InitializeComponent();
            _jsonHandlerService = new JsonHandlerService();
            _garmentService = new GarmentService(_jsonHandlerService);

            LoadGarmentsIntoDataGrid();

            txtSearch.Text = "Enter ID";
            txtSearch.Foreground = Brushes.Gray;
            txtDelete.Text = "Enter ID of garment to delete";
            txtDelete.Foreground = Brushes.Gray;
            txtUpdate.Text = "Enter ID of garment to update";
            txtUpdate.Foreground = Brushes.Gray;
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtUpdate.Text) && txtUpdate.Text != "Enter ID of garment to update")
            {
                if (uint.TryParse(txtUpdate.Text, out uint garmentID))
                {
                    var garmentToUpdate = _garmentService.SearchGarment(garmentID);
                    if (garmentToUpdate != null)
                    {
                        FormWindow_UpdateGarment formWindow = new FormWindow_UpdateGarment(_garmentService, garmentToUpdate);
                        formWindow.GarmentUpdated += UpdateGarmentForm_GarmentUpdated;
                        formWindow.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid ID.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                }
                else
                {
                    MessageBox.Show("Please enter a number.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            txtDelete.Text = "Enter ID of garment to delete";
            txtDelete.Foreground = Brushes.Gray;
        }

        private void UpdateGarmentForm_GarmentUpdated(object sender, EventArgs e)
        {
            _unsavedChanges = true;
            LoadGarmentsIntoDataGrid();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtDelete.Text) && txtDelete.Text != "Enter ID of garment to delete")
            {
                if (uint.TryParse(txtDelete.Text, out uint garmentID))
                {
                    var garmentToDelete = _garmentService.SearchGarment(garmentID);
                    if (garmentToDelete != null)
                    {
                        _garmentService.DeleteGarment(garmentID);
                        _unsavedChanges = true;
                        LoadGarmentsIntoDataGrid();
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid ID.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                }
                else
                {
                    MessageBox.Show("Please enter a number.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            // After search, reset the placeholder text
            txtDelete.Text = "Enter ID of garment to delete";
            txtDelete.Foreground = Brushes.Gray;
        }

        private void TxtDelete_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtDelete.Text == "Enter ID of garment to delete")
            {
                txtDelete.Text = ""; // Clear the placeholder text
                txtDelete.Foreground = Brushes.Black;
            }
        }
        private void TxtDelete_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDelete.Text))
            {
                txtDelete.Text = "Enter ID of garment to delete";
                txtDelete.Foreground = Brushes.Gray;
            }
        }

        private void LoadGarmentsIntoDataGrid()
        {
            // Retrieve all garments from the service
            var garments = _garmentService.GetAllGarments();

            // Bind the garments to the DataGrid
            dgGarments.ItemsSource = garments;
        }


        private string OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return null;
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            string filePath = OpenFile();
            if (!string.IsNullOrEmpty(filePath))
            {
                try
                {
                    _garmentService.LoadGarments(filePath);
                    LoadGarmentsIntoDataGrid(); // Refresh the DataGrid to show the loaded garments
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load garments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnSort_Click(object sender, RoutedEventArgs e)
        {
            SortGarmentsCriteria criteria = SortGarmentsCriteria.GarmentID;
            if (rbBrandName.IsChecked == true)
            {
                criteria = SortGarmentsCriteria.BrandName;
            }
            else if (rbPurchaseDate.IsChecked == true)
            {
                criteria = SortGarmentsCriteria.PurchaseDate;
            }
            else if (rbColor.IsChecked == true)
            {
                criteria = SortGarmentsCriteria.Color;
            }
            else if (rbSize.IsChecked == true)
            {
                criteria = SortGarmentsCriteria.Size;
            }
            else if (rbNone.IsChecked == true)
            {
                criteria = SortGarmentsCriteria.GarmentID; 
            }

            var sortedGarments = _garmentService.SortGarments(criteria);

            dgGarments.ItemsSource = sortedGarments;
        }
        private void TxtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Enter ID")
            {
                txtSearch.Text = ""; // Clear the placeholder text
                txtSearch.Foreground = Brushes.Black;
            }
        }

        private void TxtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            // If the TextBox is empty, restore the placeholder text
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Enter ID"; 
                txtSearch.Foreground = Brushes.Gray; 
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSearch.Text) && txtSearch.Text != "Enter ID")
            {
                if (uint.TryParse(txtSearch.Text, out uint garmentID))
                {
                    var foundGarment = _garmentService.SearchGarment(garmentID);
                    if (foundGarment != null)
                    {
                        dgGarments.ItemsSource = new List<Garment> { foundGarment };
                    }
                    else
                    {
                        MessageBox.Show("Garment not found. Please enter a valid ID.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a number.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            // After search, reset the placeholder text
            txtSearch.Text = "Enter ID";
            txtSearch.Foreground = Brushes.Gray;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                DefaultExt = ".json", // Default file extension
                FileName = "garments", // Default file name
            };

            // Show save file dialog box
            bool? result = saveFileDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                var filePath = saveFileDialog.FileName;

                try
                {
                    var garments = _garmentService.GetAllGarments();

                    _jsonHandlerService.SaveToFile(filePath, garments);
                    _unsavedChanges = false;

                    MessageBox.Show("Data saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AttemptCloseWindow(e);
        }

        private void AttemptCloseWindow(System.ComponentModel.CancelEventArgs e = null)
        {
            if (_unsavedChanges)
            {
                var result = MessageBox.Show("You have unsaved changes. Are you sure you want to exit?", "Unsaved Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    // Prevent the window from closing
                    if (e != null) e.Cancel = true;
                    return;
                }
            }

            // If there are no unsaved changes or the user chooses to exit, close the window
            // If called from the Exit button, e will be null, so check before setting
            if (e != null) e.Cancel = false;
            else this.Close(); // Explicitly close the window if not already closing
        }


        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            FormWindow_AddGarment formWindow = new FormWindow_AddGarment(_garmentService);
            formWindow.GarmentAdded += AddGarmentForm_GarmentAdded;
            formWindow.ShowDialog();
        }

        private void AddGarmentForm_GarmentAdded(object sender, EventArgs e)
        {
            _unsavedChanges = true;
            LoadGarmentsIntoDataGrid();
        }

        private void TxtUpdate_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtUpdate.Text == "Enter ID of garment to update")
            {
                txtUpdate.Text = ""; // Clear the placeholder text
                txtUpdate.Foreground = Brushes.Black;
            }
        }

        private void TxtUpdate_LostFocus(object sender, RoutedEventArgs e)
        {
            // If the TextBox is empty, restore the placeholder text
            if (string.IsNullOrWhiteSpace(txtUpdate.Text))
            {
                txtUpdate.Text = "Enter ID of garment to update";
                txtUpdate.Foreground = Brushes.Gray;
            }
        }
    }
}
