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

        public MainWindow()
        {
            InitializeComponent();
            IJsonHandlerService jsonHandlerService = new JsonHandlerService();
            _garmentService = new GarmentService(jsonHandlerService);

            LoadGarmentsIntoDataGrid();

            txtBrandName.Text = "Brand Name";
            txtBrandName.Foreground = Brushes.Gray;
            txtColor.Text = "Color";
            txtColor.Foreground = Brushes.Gray;
            txtSearch.Text = "Enter ID";
            txtSearch.Foreground = Brushes.Gray;
            txtDelete.Text = "Enter ID of garment to delete";
            txtDelete.Foreground = Brushes.Gray;
            txtUpdate.Text = "Enter ID of garment to update";
            txtUpdate.Foreground = Brushes.Gray;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var brandName = txtBrandName.Text.Equals("Brand Name") ? "" : txtBrandName.Text; // Check placeholder
                var purchaseDate = dpPurchaseDate.SelectedDate.HasValue ? dpPurchaseDate.SelectedDate.Value.Date : throw new InvalidOperationException("Purchase date is required.");
                var color = txtColor.Text.Equals("Color") ? "" : txtColor.Text; // Check placeholder

                // Correctly extract the size value from the ComboBox
                if (cbSize.SelectedItem == null) throw new InvalidOperationException("Size selection is required.");
                var sizeItem = cbSize.SelectedItem as ComboBoxItem;
                if (sizeItem == null) throw new InvalidOperationException("Invalid size selection.");
                var sizeValue = sizeItem.Content.ToString();
                var size = (GarmentRecordSystem.Models.Size)Enum.Parse(typeof(GarmentRecordSystem.Models.Size), sizeValue);

                // Ensure non-placeholder values are provided
                if (string.IsNullOrWhiteSpace(brandName) || string.IsNullOrWhiteSpace(color))
                {
                    throw new InvalidOperationException("Brand name and color are required.");
                }

                var garment = new Garment(brandName, new DateOnly(purchaseDate.Year, purchaseDate.Month, purchaseDate.Day), color, size);
                _garmentService.AddGarment(garment);

                LoadGarmentsIntoDataGrid(); // Refresh the list showing garments
                MessageBox.Show("Garment added successfully.");

                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Implementation for updating an existing garment record
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

        private void LoadGarmentsIntoDataGrid()
        {
            // Retrieve all garments from the service
            var garments = _garmentService.GetAllGarments();

            // Bind the garments to the DataGrid
            dgGarments.ItemsSource = garments;
        }

        private void Txt_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            // Assuming you set the initial text to your placeholder text
            if (textBox != null && (textBox.Text == "Brand Name" || textBox.Text == "Color"))
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black; // Reset to default text color
            }
        }

        private void Txt_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Foreground = Brushes.Gray; // Placeholder text color
                textBox.Text = textBox.Name == "txtBrandName" ? "Brand Name" : "Color"; // Set the placeholder text based on the TextBox's name
            }
        }

        private void ClearInputFields()
        {
            // Reset the input fields to their initial state or placeholder text
            txtBrandName.Text = "Brand Name";
            txtBrandName.Foreground = Brushes.Gray;

            dpPurchaseDate.SelectedDate = null; // Clear the date selection

            txtColor.Text = "Color";
            txtColor.Foreground = Brushes.Gray;

            cbSize.SelectedIndex = -1; // Clear the combo box selection
        }

        private string OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) // Or any specific directory
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
                        MessageBox.Show("Please enter a valid ID.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }

            // After search, reset the placeholder text
            txtSearch.Text = "Enter ID";
            txtSearch.Foreground = Brushes.Gray;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
