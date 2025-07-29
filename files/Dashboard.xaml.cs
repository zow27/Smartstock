using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SmartStock.Data;


namespace SmartStock
{
    public partial class Dashboard : Window
    {

        private ObservableCollection<Item> Items { get; set; }

      
        private ICollectionView itemsView;

    
        private int nextId = 1;

    
        private Item selectedItemForEdit = null;

        public Dashboard()
        {
            InitializeComponent();

           
            Items = new ObservableCollection<Item>
            {
               
            };

          
            itemsView = CollectionViewSource.GetDefaultView(Items);
            ItemsDataGrid.ItemsSource = itemsView;
        }


        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = SearchTextBox.Text.Trim().ToLower();
            itemsView.Filter = string.IsNullOrEmpty(filter)
                ? (Predicate<object>)null
                : (obj => ((Item)obj).Name.ToLower().Contains(filter));
        }

   
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

                Items.Add(new Item {Name = name, Quantity = qty });
            }
            else
            {
     
                selectedItemForEdit.Name = name;
                selectedItemForEdit.Quantity = qty;
                itemsView.Refresh();
                selectedItemForEdit = null;
            }

            ClearSelection();
        }

    
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


        private void ClearSelection_Click(object sender, RoutedEventArgs e)
            => ClearSelection();

        private void ClearSelection()
        {
            ItemsDataGrid.UnselectAll();
            NameInput.Text = "";
            QuantityInput.Text = "";
            selectedItemForEdit = null;
          
        }


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
