using GarmentRecordSystem.Models;
using GarmentRecordSystem.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WPFbasedUI
{
    /// <summary>
    /// Interaction logic for FormWindow_UpdateGarment.xaml
    /// </summary>
    public partial class FormWindow_UpdateGarment : Window
    {
        private readonly IGarmentService _garmentService;
        private Garment _garment;
        public event EventHandler GarmentUpdated;

        public FormWindow_UpdateGarment(IGarmentService garmentService, Garment garment)
        {
            InitializeComponent();

            _garmentService = garmentService;
            _garment = garment;

            //Display current values
            txtBrandName.Text = _garment.BrandName;
            dpPurchaseDate.SelectedDate = _garment.PurchaseDate.ToDateTime(new TimeOnly(0, 0));
            txtColor.Text = _garment.Color;

            foreach (ComboBoxItem item in cbSize.Items)
            {
                if (item.Content.ToString() == _garment.Size.ToString())
                {
                    cbSize.SelectedItem = item;
                    break;
                }
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newBrandName = txtBrandName.Text;
                var newPurchaseDate = DateOnly.FromDateTime((DateTime)dpPurchaseDate.SelectedDate);
                var newColor = txtColor.Text;
                var sizeItem = cbSize.SelectedItem as ComboBoxItem;
                var sizeValue = sizeItem.Content.ToString();
                var newSize = (GarmentRecordSystem.Models.Size)Enum.Parse(typeof(GarmentRecordSystem.Models.Size), sizeValue);

                if (string.IsNullOrWhiteSpace(newBrandName) || string.IsNullOrWhiteSpace(newColor))
                {
                    throw new InvalidOperationException("Brand name and color are required.");
                }

                _garmentService.UpdateGarment(_garment.GarmentID,
                    newBrandName, newPurchaseDate, newColor, newSize);

                MessageBox.Show("Garment updated successfully.");

                OnGarmentUpdated();
                this.Close();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Update failed.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected virtual void OnGarmentUpdated()
        {
            GarmentUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
