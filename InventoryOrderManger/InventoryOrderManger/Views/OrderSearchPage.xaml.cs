using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryOrderManger.Common;
using InventoryOrderManger.Database;
using InventoryOrderManger.Models;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static InventoryOrderManger.Common.Enumerations;

namespace InventoryOrderManger.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderSearchPage : ContentPage
    {
        private DbConnection dbConnection = DbConnection.GetDbConnection();
        public ObservableCollection<OrderHeader> OrderHeaders { get; set; }
        private List<OrderHeader> _orderHeaders;
        public OrderSearchPage()
        {
            InitializeComponent();
            OrderHeaders = new ObservableCollection<OrderHeader>();

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadItemsFromDB();
        }

        private async void LoadItemsFromDB()
        {
            try
            {
                _orderHeaders = await dbConnection.GetOrderHeaders();

                OrderHeaders.Clear();
                OrderHeaders.AddRange(_orderHeaders.OrderByDescending(x => x.CreatedDate));
            }
            catch (SQLiteException ee)
            {
                await DisplayAlert("Try restarting App", $"Somthing went wrong. {ee.Message}", "OK");
            }

        }

        private void OnSearch(object sender, EventArgs e)
        {
            FilterItems(this.searchBar.Text);
        }

        private void CreateNewOrder(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OrderPage(Enumerations.OperationType.Create));
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            OrderHeader item = e.Item as OrderHeader;
            Navigation.PushAsync(new OrderPage(OperationType.Update, item));
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            this.listView.SelectedItem = null;
        }

        private void FilterItems(string text)
        {
            if (!string.IsNullOrWhiteSpace(text) && _orderHeaders != null)
            {
                OrderHeaders.Clear();
                OrderHeaders.AddRange(_orderHeaders.Where(x => x.CustomerName.ToLower().Contains(text.ToLower())));
            }
            else
            {
                OrderHeaders.Clear();
                OrderHeaders.AddRange(_orderHeaders);
            }
        }

        private void OnSearch(object sender, TextChangedEventArgs e)
        {
            FilterItems(e.NewTextValue);
        }
    }
}