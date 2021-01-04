using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryOrderManger.Common;
using InventoryOrderManger.Database;
using InventoryOrderManger.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InventoryOrderManger.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPage : ContentPage, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler PropertyChanged;
        
        public ObservableCollection<OrderLine> OrderLines { get; set; }
        private OrderHeader _orderHeader;
        private bool areLinesLoaded = false;
        public OrderHeader OrderHeader
        {
            get
            {
                return _orderHeader;
            }
            set
            {
                _orderHeader = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrderHeader)));
            }
        }

        private DbConnection dbConnection = DbConnection.GetDbConnection();
        public OrderPage()
        {
            InitializeComponent();
            OrderLines = new ObservableCollection<OrderLine>();
            OrderHeader = new OrderHeader();

            BindingContext = this;
        }

        public OrderPage(Enumerations.OperationType operationType, OrderHeader header = null) : this()
        {
            if (header != null)
            {
                OrderHeader = header;
            }

            SetControlVisibility(operationType);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (OrderHeader.OrderID != 0 && !areLinesLoaded)
            {
                areLinesLoaded = true;
                var lines = await dbConnection.GetOrderLines();
                OrderLines.Clear();
                OrderLines.AddRange(lines.Where(x => x.OrderID == OrderHeader.OrderID));
            }
        }

        private void OnBack(object sender, EventArgs e)
        {
            var backButton = sender as Button;
            backButton.IsEnabled = false;
            Navigation.PopAsync();
        }

        private void OnClear(object sender, EventArgs e)
        {
            ClearControls();
        }

        private void ClearControls()
        {
            OrderLines.Clear();
            OrderHeader = new OrderHeader();
        }      

        private void OnAddItem(object sender, EventArgs e)
        {
            ItemSelectPage page = new ItemSelectPage();
            Navigation.PushAsync(page);

            page.ItemsSelected += OnItemsSelected;
        }

        private void OnItemsSelected(object sender, ItemSelectedEventArgs e)
        {
            foreach(Item item in e.SelectedItems)
            {
                OrderLines.Add(new OrderLine() 
                {
                    ItemID = item.ItemID,
                    ItemName = item.ItemName,
                    ItemSellPrice = item.SellPrice,
                    ItemOrderQty = 1,
                    ItemTotalPrice = item.SellPrice
                });
            }

            CalculateOrderTotalPrice();
        }

        private async void OnSave(object sender, EventArgs e)
        {
            if (OrderHeader.OrderID != 0)
            {
                await dbConnection.UpdateRecord(OrderHeader);

                foreach (OrderLine line in OrderLines)
                {
                    line.OrderID = OrderHeader.OrderID;
                }

                // for lines may be need to add create and update seperately and 
                // Lines also might need a primary key

                await dbConnection.UpdateRecord(OrderLines.Where(x => x.OrderLineID != 0).ToList());
                await dbConnection.InsertRecord(OrderLines.Where(x => x.OrderLineID == 0).ToList());

                await DisplayAlert("Success", $"{OrderHeader.OrderNo} updated.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                OrderHeader.OrderNo = await SequenceGenerator.GetSequenceNo(Enumerations.SequenceType.SO);
                await dbConnection.InsertRecord(OrderHeader);

                foreach (OrderLine line in OrderLines)
                {
                    line.OrderID = OrderHeader.OrderID;
                }

                await dbConnection.InsertRecord(OrderLines.ToList());
                await DisplayAlert("Success", $"{OrderHeader.OrderNo} created.", "OK");
            }

            ClearControls();
        }

        private void SetControlVisibility(Enumerations.OperationType operationType)
        {
            switch (operationType)
            {
                case Enumerations.OperationType.Create:
                    //this.itemImage.IsVisible = true;
                    //this.description.IsVisible = true;
                    //this.purchasePrice.IsVisible = true;
                    //this.stockQty.IsVisible = true;
                    //this.btnClear.IsVisible = true;
                    this.Title = "Create Order";
                    break;
                case Enumerations.OperationType.Update:
                    //this.itemImage.IsVisible = true;
                    //this.description.IsVisible = true;
                    //this.purchasePrice.IsVisible = true;
                    //this.stockQty.IsVisible = true;
                    //this.btnClear.IsVisible = false;
                    this.Title = "Update Order";
                    break;
                default:
                    break;
            }
        }

        private void OnQuantityValueChange(object sender, EventArgs e)
        {
            QuantityView quantity = sender as QuantityView;
            OrderLine line = quantity.CommandParameter as OrderLine;

            if (string.IsNullOrWhiteSpace(quantity.QuantityText) || Convert.ToDecimal(quantity.QuantityText) <= 0)
            {
                OrderLines.Remove(line);
            }
            else
            {
                line.ItemTotalPrice = line.ItemSellPrice * line.ItemOrderQty;
            }

            CalculateOrderTotalPrice();
        }

        private void CalculateOrderTotalPrice()
        {
            OrderHeader.OrderTotalPrice = OrderLines.Select(x => x.ItemTotalPrice).Sum();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListView view = sender as ListView;
            view.SelectedItem = null;
        }
    }
}