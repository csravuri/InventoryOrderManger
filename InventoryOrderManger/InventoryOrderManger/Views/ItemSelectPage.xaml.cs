using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using InventoryOrderManger.Common;
using InventoryOrderManger.Database;
using InventoryOrderManger.Models;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InventoryOrderManger.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemSelectPage : ContentPage
    {
        public ObservableCollection<Item> Items { get; set; }
        private DbConnection dbConnection = DbConnection.GetDbConnection();
        private List<Item> _selectedItems;

        public event EventHandler<ItemSelectedEventArgs> ItemsSelected;

        public ItemSelectPage()
        {
            InitializeComponent();
            _selectedItems = new List<Item>();
            Items = new ObservableCollection<Item>();
            BindingContext = this;
        }

        private async void LoadItemsFromDB()
        {
            try
            {
                var itemFromDB = await dbConnection.GetItems();

                Items.Clear();
                Items.AddRange(itemFromDB);

                Items.Where(x => _selectedItems.Select(y => y.ID).Contains(x.ID)).ToList().ForEach(z => z.IsSelected = true);
            }
            catch (SQLiteException ee)
            {
                await DisplayAlert("Try restarting App", $"Somthing went wrong. {ee.Message}", "OK");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadItemsFromDB();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _selectedItems = Items.Where(x => x.IsSelected).ToList();
        }

        private void CreateNewStock(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ItemPage(Common.Enumerations.OperationType.ExpressCreate));
        }

        private void OnSearch(object sender, TextChangedEventArgs e)
        {
            FilterItems(e.NewTextValue);
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Item item = e.Item as Item;
            Navigation.PushAsync(new ItemPage(Common.Enumerations.OperationType.Update, item));
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            this.listView.SelectedItem = null;
        }

        private void OnItemDelete(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            Item selectedItem = menuItem.CommandParameter as Item;

            Items.Remove(selectedItem);
            _ = dbConnection.DeleteRecord(selectedItem);
        }

        private void OnSearch(object sender, EventArgs e)
        {
            FilterItems(this.searchBar.Text);
        }

        private void FilterItems(string text)
        {
            if (!string.IsNullOrWhiteSpace(text) && Items != null)
            {
                this.listView.ItemsSource = Items.Where(x => text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Any(t => x.ItemName.ToLower().Contains(t.ToLower())));
            }
            else
            {
                this.listView.ItemsSource = Items;
            }
        }

        private void OnBack(object sender, EventArgs e)
        {
            var backButton = sender as Button;
            backButton.IsEnabled = false;
            Navigation.PopAsync();
        }

        private void OnDone(object sender, EventArgs e)
        {
            ItemsSelected?.Invoke(this, new ItemSelectedEventArgs() { SelectedItems = Items.Where(x => x.IsSelected).ToList() });
            Navigation.PopAsync();
        }

        private void OnAdd(object sender, EventArgs e)
        {
            Button addButton = sender as Button;
            Item item = addButton.CommandParameter as Item;

            item.IsSelected = !item.IsSelected;
        }
    }

    public class ItemSelectedEventArgs : EventArgs
    {
        public List<Item> SelectedItems { get; set; }
    }
}