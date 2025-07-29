using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SmartStock.Data;  // <-- Make sure this matches your DbContext & Item namespace


namespace SmartStock
{
    public partial class Dashboard : Window
    {
        // Backing collection
        private ObservableCollection<Item> Items { get; set; }

        // Allows filtering/search
        private ICollectionView itemsView;

        // Next ID (for in‑memory demo; replace with DB logic later)
        private int nextId = 1;

        // Track item being edited
        private Item selectedItemForEdit = null;

        public Dashboard()
        {
            InitializeComponent();

            // If you have a database, replace this block with:
            // using(var db = new SmartStockDbContext(...)) { Load from db.Items.ToList(); }

            // Sample startup items
            Items = new ObservableCollection<Item>
            {
               
            };

            // Set up filtering view
            itemsView = CollectionViewSource.GetDefaultView(Items);
            ItemsDataGrid.ItemsSource = itemsView;
        }

        // Search bar filter
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = SearchTextBox.Text.Trim().ToLower();
            itemsView.Filter = string.IsNullOrEmpty(filter)
                ? (Predicate<object>)null
                : (obj => ((Item)obj).Name.ToLower().Contains(filter));
        }

        // Add or update logic
        private void AddOrUpdateItem_Click(object sender, RoutedEventArgs e)
        {
            string name = NameInput.Text.Trim();
            if (!int.TryParse(QuantityInput.Text.Trim(), out int qty))
            {
                MessageBox.Show("Please enter a valid quantity.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedItemForEdit == null)
            {
                // Add new
                Items.Add(new Item {Name = name, Quantity = qty });
            }
            else
            {
                // Update existing
                selectedItemForEdit.Name = name;
                selectedItemForEdit.Quantity = qty;
                itemsView.Refresh();
                selectedItemForEdit = null;
            }

            ClearSelection();
        }

        // Delete logic
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsDataGrid.SelectedItem is Item sel)
            {
                Items.Remove(sel);
                ClearSelection();
            }
            else
            {
                MessageBox.Show("Please select an item to delete.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Clear inputs & selection
        private void ClearSelection_Click(object sender, RoutedEventArgs e)
            => ClearSelection();

        private void ClearSelection()
        {
            ItemsDataGrid.UnselectAll();
            NameInput.Text = "";
            QuantityInput.Text = "";
            selectedItemForEdit = null;
            // Reset button text if you changed it for edit mode
        }

        // When user selects a row, populate inputs for edit
        private void ItemsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ItemsDataGrid.SelectedItem is Item item)
            {
                selectedItemForEdit = item;
                NameInput.Text = item.Name;
                QuantityInput.Text = item.Quantity.ToString();
            }
        }
    }
}
