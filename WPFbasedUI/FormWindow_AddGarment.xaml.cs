﻿using GarmentRecordSystem.Models;
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
using System.Windows.Shapes;

namespace WPFbasedUI
{
    /// <summary>
    /// Interaction logic for FormWindow_AddGarment.xaml
    /// </summary>
    public partial class FormWindow_AddGarment : Window
    {
        private readonly IGarmentService _garmentService;
        public event EventHandler GarmentAdded;
        public FormWindow_AddGarment(IGarmentService garmentService)
        {
            InitializeComponent();

            _garmentService = garmentService;

            txtBrandName.Text = "Brand Name";
            txtBrandName.Foreground = Brushes.Gray;
            txtColor.Text = "Color";
            txtColor.Foreground = Brushes.Gray;
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

                MessageBox.Show("Garment added successfully.");
                ClearInputFields();
                OnGarmentAdded();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
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

        protected virtual void OnGarmentAdded()
        {
            GarmentAdded?.Invoke(this, EventArgs.Empty);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
