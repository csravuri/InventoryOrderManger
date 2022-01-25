using InventoryOrderManger.Common;
using InventoryOrderManger.Database;
using InventoryOrderManger.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        private readonly Enumerations.OperationType operationType;

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
            this.operationType = operationType;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (OrderHeader.ID != Guid.Empty && !areLinesLoaded)
            {
                areLinesLoaded = true;
                var lines = await dbConnection.GetOrderLines();
                OrderLines.Clear();
                OrderLines.AddRange(lines.Where(x => x.OrderID == OrderHeader.ID));
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
            foreach (Item item in e.SelectedItems)
            {
                OrderLines.Add(new OrderLine()
                {
                    ItemID = item.ID,
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
            if (!ValidateOrders())
            {
                return;
            }

            if (OrderHeader.ID != Guid.Empty)
            {
                await dbConnection.UpdateRecord(OrderHeader);

                foreach (OrderLine line in OrderLines)
                {
                    line.OrderID = OrderHeader.ID;
                }

                await dbConnection.UpdateRecord(OrderLines.Where(x => x.ID != Guid.Empty).ToList());
                await dbConnection.InsertRecord(OrderLines.Where(x => x.ID == Guid.Empty).ToList());

                await DisplayAlert("Success", $"{OrderHeader.OrderNo} updated.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                OrderHeader.OrderNo = await SequenceGenerator.GetSequenceNo(Enumerations.SequenceType.SO);
                await dbConnection.InsertRecord(OrderHeader);

                foreach (OrderLine line in OrderLines)
                {
                    line.OrderID = OrderHeader.ID;
                }

                await dbConnection.InsertRecord(OrderLines.ToList());
                await DisplayAlert("Success", $"{OrderHeader.OrderNo} created.", "OK");
            }

            ClearControls();
        }

        private bool ValidateOrders()
        {
            if (OrderLines == null || OrderLines.Count == 0)
            {
                DisplayAlert("Error", "Add Items to save.", "OK");
                return false;
            }

            return true;
        }

        private void SetControlVisibility(Enumerations.OperationType operationType)
        {
            switch (operationType)
            {
                case Enumerations.OperationType.Create:
                    this.Title = "Create Order";
                    break;

                case Enumerations.OperationType.Update:
                    this.btnClear.IsVisible = false;
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