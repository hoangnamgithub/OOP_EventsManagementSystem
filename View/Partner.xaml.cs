using OOP_EventsManagementSystem.Styles;
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
using OOP_EventsManagementSystem.ViewModel;
using OOP_EventsManagementSystem.Model;
using System.Windows.Controls.Primitives;
using Microsoft.Extensions.Logging;


namespace OOP_EventsManagementSystem.View
{
    /// <summary>
    /// Interaction logic for Partner.xaml
    /// </summary>
    public partial class Partner : UserControl
    {
        private readonly EventManagementDbContext _context;
        private PartnerVM _viewModel;
        public Partner()
        {
            InitializeComponent();
            // Khởi tạo DbContext
            _context = new EventManagementDbContext();

            // Khởi tạo ViewModel
            _viewModel = new PartnerVM(_context);

            // Đặt DataContext của cửa sổ với ViewModel
            DataContext = _viewModel;
        }

        private void SponsorNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // The SelectedSponsor property will automatically update
            // The details of the selected sponsor will be reflected in the TextBlock
        }
        // Xử lý sự kiện chọn sự kiện từ DataGrid
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedEvent = (EventModel)((DataGrid)sender).SelectedItem;

            if (selectedEvent != null)
            {
                _viewModel.SelectedEventId = selectedEvent.EventId;
                _viewModel.LoadSponsorsForSelectedEvent();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
           
            // Make Description Border visible if all fields are filled
            Description.Visibility = Visibility.Visible;

                // Optionally clear previous inputs (this can be done after successful input validation)
                TextBoxName.Clear();
                TextBoxDetails.Clear();
            SponsorTierComboBox.ClearValue(ComboBox.SelectedItemProperty);

                // Scroll to the end of the ScrollViewer
                if (DescriptionScrollViewer != null)
                {
                    DescriptionScrollViewer.ScrollToEnd();
                }

                // Optionally set the default value for SponsorTierName if required
            
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            // Make Select_Sponsor Border visible
            Select_Sponsor.Visibility = Visibility.Visible;

            // Optionally clear previous inputs
            SponsorNameComboBox.SelectedItem = null;
            TextBoxDetails.Clear();
            SponsorTierCb_box.SelectedItem = null;

            if (DescriptionScrollViewer != null)
            {
                DescriptionScrollViewer.ScrollToEnd();
            }
        }

        private bool isEditing = false; // Flag to track editing state
        private object currentSelectedItem = null; // The item currently being edited

        private bool isSelectionChangeAllowed = true; // Flag to control selection change

        // Edit button click event
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SponsorDataGrid.SelectedItem != null)
            {
                currentSelectedItem = SponsorDataGrid.SelectedItem; // Record the selected row
                foreach (var column in SponsorDataGrid.Columns)
                {
                    if (column is DataGridTextColumn textColumn)
                    {
                        textColumn.IsReadOnly = false; // Allow editing
                    }
                }

                SponsorDataGrid.IsReadOnly = false; // Enable grid editing
                isEditing = true; // Set editing state

                EditButton.IsEnabled = false; // Disable Edit button
                EditButton.Visibility = Visibility.Collapsed;
                ConfirmButton.IsEnabled = true; // Enable Confirm button
                ConfirmButton.Visibility = Visibility.Visible;

            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hàng để chỉnh sửa.", "Thông báo"); // Prompt if no row is selected
            }
        }

        // Confirm button click event
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentSelectedItem != null)
            {
                try
                {
                    // Prevent selection change during confirmation
                    isSelectionChangeAllowed = false;

                    // Assuming currentSelectedItem is of type SponsorModel
                    var sponsorToSave = currentSelectedItem as SponsorModel;

                    if (sponsorToSave != null)
                    {
                        // Save changes using the ViewModel's SaveChanges method
                        var partnerVM = (PartnerVM)DataContext; // Get the current ViewModel
                        partnerVM.SaveChanges(sponsorToSave); // Save the sponsor changes

                        // After saving, lock the data grid and reset UI states
                        foreach (var column in SponsorDataGrid.Columns)
                        {
                            // Make sure ID column is read-only
                            if (column.Header.ToString() == "ID") // or use another condition to identify the ID column
                            {
                                column.IsReadOnly = true;
                            }
                            else if (column is DataGridTextColumn textColumn)
                            {
                                textColumn.IsReadOnly = true; // Lock editing for all other columns
                            }
                        }

                        SponsorDataGrid.IsReadOnly = true; // Lock DataGrid
                        isEditing = false; // Disable editing mode
                        currentSelectedItem = null; // Clear current selection

                        EditButton.IsEnabled = true; // Re-enable Edit button
                        EditButton.Visibility = Visibility.Visible;

                        ConfirmButton.IsEnabled = false; // Disable Confirm button
                        ConfirmButton.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        MessageBox.Show("Invalid sponsor selected.", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error");
                }
                finally
                {
                    // Re-enable selection change after the save operation
                    isSelectionChangeAllowed = true;
                }
            }
        }

        // Selection changed event for preventing selection change while editing
        private void SponsorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if selection change is allowed
            if (!isSelectionChangeAllowed || (isEditing && currentSelectedItem != null && SponsorDataGrid.SelectedItem != currentSelectedItem))
            {
                
                SponsorDataGrid.SelectedItem = currentSelectedItem; // Reset to the current selected item
            }
        }

        // Beginning edit event to prevent editing on non-selected rows
        private void SponsorDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (!isEditing || e.Row.Item != currentSelectedItem)
            {
                e.Cancel = true; // Cancel edit if not the row currently being edited
            }
        }



        private void ConfirmAddButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if required fields are filled
            if (string.IsNullOrWhiteSpace(TextBoxName.Text) ||
                string.IsNullOrWhiteSpace(TextBoxDetails.Text) ||
                string.IsNullOrWhiteSpace(SponsorTierComboBox.SelectedValue?.ToString()))
            {
                // Show a reminder message if any field is empty
                MessageBox.Show("Please fill in all the required information (Name, Details, and Sponsor Tier) before adding the sponsor.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                try
                {
                    var sponsorVM = (PartnerVM)DataContext;  // Get the ViewModel

                    // Get the data from the UI
                    string sponsorName = TextBoxName.Text;
                    string sponsorDetails = TextBoxDetails.Text;
                    string sponsorTierName = SponsorTierComboBox.SelectedValue.ToString();
                    int selectedEventId = _viewModel.SelectedEventId;  // Assuming this is the ID of the selected event

                    // Call the AddNewSponsor method in the ViewModel
                    sponsorVM.AddNewSponsor(sponsorName, sponsorDetails, sponsorTierName, selectedEventId);

                    // Hide the description border after adding
                    Description.Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error");
                }
            }
        }

        private void ConfirmAddExistButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (PartnerVM)this.DataContext;

            if (viewModel.SelectedSponsor != null && viewModel.SelectedSponsorTier != null)
            {
                viewModel.AddExistingSponsorToEvent(viewModel.SelectedSponsor, viewModel.SelectedEventId, viewModel.SelectedSponsorTier.TierName);
            }
            else
            {
                MessageBox.Show("Please select both a sponsor and a sponsor tier.");
            }

        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem có sponsor nào được chọn trong DataGrid không
            var selectedSponsor = SponsorDataGrid.SelectedItem as SponsorModel;
            if (selectedSponsor != null)
            {
                // Hiển thị MessageBox xác nhận xóa
                var result = MessageBox.Show($"Are you sure you want to delete {selectedSponsor.SponsorName}?",
                                              "Confirm Delete",
                                              MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    // Gọi hàm xóa từ ViewModel
                    var partnerVM = (PartnerVM)DataContext;
                    partnerVM.DeleteSponsorFromEvent(selectedSponsor);
                }
            }
            else
            {
                MessageBox.Show("Please select a sponsor to delete.");
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Choose.IsOpen = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Description.Visibility = Visibility.Collapsed;
            Select_Sponsor.Visibility = Visibility.Collapsed;
        }
    }
}
