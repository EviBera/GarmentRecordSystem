using GarmentRecordSystem.Models;
using GarmentRecordSystem.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFbasedUI
{
    /// <summary>
    /// Interaction logic for FormWindow_AddGarment.xaml
    /// </summary>
    public partial class FormWindow_AddGarment : Window
    {
        private readonly IGarmentService _garmentService;
        private readonly Placeholders _placeholders;
        public event EventHandler GarmentAdded;

        public FormWindow_AddGarment(IGarmentService garmentService)
        {
            InitializeComponent();

            _garmentService = garmentService;
            _placeholders = new Placeholders();

            SetBrandNameText();
            SetColorText();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var brandName = txtBrandName.Text.Equals(_placeholders.BrandName) ? "" : txtBrandName.Text; // Check placeholder
                var purchaseDate = dpPurchaseDate.SelectedDate.HasValue ? dpPurchaseDate.SelectedDate.Value.Date : throw new InvalidOperationException("Purchase date is required.");
                var color = txtColor.Text.Equals(_placeholders.Color) ? "" : txtColor.Text; // Check placeholder

                // Correctly extract the size value from the ComboBox
                if (cbSize.SelectedItem == null) throw new InvalidOperationException("Size selection is required.");
                var sizeItem = cbSize.SelectedItem as ComboBoxItem;
                if (sizeItem == null) throw new InvalidOperationException("Invalid size selection.");
                var sizeValue = sizeItem.Content.ToString();
                var size = (GarmentRecordSystem.Models.Size)Enum.Parse(typeof(GarmentRecordSystem.Models.Size), sizeValue);

                // Ensure real values are provided
                if (string.IsNullOrWhiteSpace(brandName) || string.IsNullOrWhiteSpace(color))
                {
                    throw new InvalidOperationException("Brand name and color are required.");
                }

                var garment = new Garment(brandName, new DateOnly(purchaseDate.Year, purchaseDate.Month, purchaseDate.Day), color, size);
                _garmentService.AddGarment(garment);

                MessageBox.Show("Garment added successfully.");
                ClearInputFields();
                OnGarmentAdded();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void Txt_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null && (textBox.Text == _placeholders.BrandName || textBox.Text == _placeholders.Color))
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
                if(textBox.Name == "txtBrandName")  // Set the placeholder text based on the TextBox's name
                {
                    SetBrandNameText();
                } 
                else SetColorText(); 
            }
        }
        
        private void ClearInputFields()
        {
            SetBrandNameText();
            dpPurchaseDate.SelectedDate = null; // Clear the date selection
            SetColorText();
            cbSize.SelectedIndex = -1; // Clear the combo box selection
        }

        protected virtual void OnGarmentAdded()
        {
            GarmentAdded?.Invoke(this, EventArgs.Empty);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SetBrandNameText()
        {
            txtBrandName.Text = _placeholders.BrandName;
            txtBrandName.Foreground = Brushes.Gray;
        }
        private void SetColorText()
        {
            txtColor.Text = _placeholders.Color;
            txtColor.Foreground = Brushes.Gray;
        }
    }
}

