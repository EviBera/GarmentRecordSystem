using GarmentRecordSystem.Models;
using GarmentRecordSystem.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;


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
        private Placeholders _placeholders;


        public MainWindow()
        {
            InitializeComponent();

            _jsonHandlerService = new JsonHandlerService();
            _garmentService = new GarmentService(_jsonHandlerService);
            _placeholders = new Placeholders();

            LoadGarmentsIntoDataGrid();

            SetSearchText();
            SetDeleteText();
            SetUpdateText();
        }


        //Handle load button
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


        //Handle sorting
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


        //Handle add section
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


        //Handle search section
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSearch.Text) && txtSearch.Text != _placeholders.SearchText)
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

            SetSearchText();
        }

        private void TxtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == _placeholders.SearchText)
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
                SetSearchText();
            }
        }


        //Handle update section
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtUpdate.Text) && txtUpdate.Text != _placeholders.UpdateText)
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
            SetUpdateText();
        }

        private void UpdateGarmentForm_GarmentUpdated(object sender, EventArgs e)
        {
            _unsavedChanges = true;
            LoadGarmentsIntoDataGrid();
        }

        private void TxtUpdate_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtUpdate.Text == _placeholders.UpdateText)
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
                SetUpdateText();
            }
        }


        //Handle delete section
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtDelete.Text) && txtDelete.Text != _placeholders.DeleteText)
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
            SetDeleteText();
        }

        private void TxtDelete_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtDelete.Text == _placeholders.DeleteText)
            {
                txtDelete.Text = ""; // Clear the placeholder text
                txtDelete.Foreground = Brushes.Black;
            }
        }
        private void TxtDelete_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDelete.Text))
            {
                SetDeleteText();
            }
        }


        //Handle save button
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


        //Handle exit button
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Closing the app
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

            // If there aren't any unsaved changes or the user chooses to exit, close the window
            // If called from the Exit button, e will be null, so check before setting
            if (e != null) e.Cancel = false;
            else this.Close(); // Explicitly close the window if not already closing
        }


        //Further helper methods
        private void LoadGarmentsIntoDataGrid()
        {
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

        private void SetSearchText()
        {
            txtSearch.Text = _placeholders.SearchText;
            txtSearch.Foreground = Brushes.Gray;
        }
        private void SetDeleteText()
        {
            txtDelete.Text = _placeholders.DeleteText;
            txtDelete.Foreground = Brushes.Gray;
        }

        private void SetUpdateText()
        {
            txtUpdate.Text = _placeholders.UpdateText;
            txtUpdate.Foreground = Brushes.Gray;
        }
    }
}
