using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryOrderManger.Database;
using InventoryOrderManger.Models;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static InventoryOrderManger.Common.Enumerations;

namespace InventoryOrderManger.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemSearchPage : ContentPage
    {
        private ObservableCollection<Item> _itemsFromDB;
        private DbConnection dbConnection = DbConnection.GetDbConnection();
        public ItemSearchPage()
        {
            InitializeComponent();
        }

        private async void LoadItemsFromDB()
        {
            try
            {
                var abc = await dbConnection.GetItems();

                _itemsFromDB = new ObservableCollection<Item>(abc);
                this.listView.ItemsSource = _itemsFromDB;
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

        private void CreateNewStock(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ItemPage(OperationType.Create));
        }

        private void OnSearch(object sender, TextChangedEventArgs e)
        {
            FilterItems(e.NewTextValue);
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Item item = e.Item as Item;
            Navigation.PushAsync(new ItemPage(OperationType.Update, item));

        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            this.listView.SelectedItem = null;
        }

        private void OnItemDelete(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            Item selectedItem = menuItem.CommandParameter as Item;

            _itemsFromDB.Remove(selectedItem);
            dbConnection.DeleteItem(selectedItem);
        }

        private void OnSearch(object sender, EventArgs e)
        {
            FilterItems(this.searchBar.Text);
        }

        private void FilterItems(string text)
        {
            if (!string.IsNullOrWhiteSpace(text) && _itemsFromDB != null)
            {
                this.listView.ItemsSource = _itemsFromDB.Where(x => x.ItemName.ToLower().Contains(text.ToLower()));
            }
            else
            {
                this.listView.ItemsSource = _itemsFromDB;
            }
        }
    }
}