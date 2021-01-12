using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryOrderManger.Common;
using InventoryOrderManger.Database;
using InventoryOrderManger.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InventoryOrderManger.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemPage : ContentPage
    {
        private Item _item = null;
        private DbConnection dbConnection = DbConnection.GetDbConnection();
        public ItemPage()
        {
            InitializeComponent();
        }

        public ItemPage(Enumerations.OperationType operationType, Item item = null) : this()
        {
            _item = item;
            SetPageData(_item);
            SetControlVisibility(operationType);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void SetControlVisibility(Enumerations.OperationType operationType)
        {
            switch (operationType)
            {
                case Enumerations.OperationType.Create:
                    this.itemImage.IsVisible = true;
                    this.description.IsVisible = true;
                    this.purchasePrice.IsVisible = true;
                    this.stockQty.IsVisible = true;
                    this.btnClear.IsVisible = true;
                    this.Title = "Create Item";
                    break;
                case Enumerations.OperationType.Update:
                    this.itemImage.IsVisible = true;
                    this.description.IsVisible = true;
                    this.purchasePrice.IsVisible = true;
                    this.stockQty.IsVisible = true;
                    this.btnClear.IsVisible = false;
                    this.Title = "Update Item";
                    break;
                case Enumerations.OperationType.ExpressCreate:
                    this.itemImage.IsVisible = false;
                    this.description.IsVisible = false;
                    this.purchasePrice.IsVisible = false;
                    this.stockQty.IsVisible = false;
                    this.btnClear.IsVisible = true;
                    this.Title = "Express Create Item";
                    break;
                default:
                    break;
            }
        }

        private async void CaptureImage(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            try
            {
                // https://github.com/jamesmontemagno/MediaPlugin
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Full,
                    Name = Utils.GetDateTimeFileName(".jpg"),
                    AllowCropping = true,
                    MaxWidthHeight = 100
                });

                if (file == null)
                    return;

                _item.ImagePath = file.Path;
                this.itemImage.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.Message, "OK");
            }
        }

        private void OnClear(object sender, EventArgs e)
        {
            ClearControls();
        }

        private async void OnSave(object sender, EventArgs e)
        {
            if (IsFormValid())
            {
                _item = GetPageData();

                if (_item.ItemID != 0)
                {
                    await dbConnection.UpdateRecord(_item);
                    await DisplayAlert("Success", $"{_item.ItemName} updated.", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await dbConnection.InsertRecord(_item);
                    await DisplayAlert("Success", $"{_item.ItemName} created.", "OK");
                }

                ClearControls();
            }
        }

        private bool IsFormValid()
        {
            if (string.IsNullOrWhiteSpace(this.itemName.Text))
            {
                DisplayAlert("Error", $"{this.itemName.Placeholder} is Required!", "OK");
                return false;
            }

            if (!Utils.IsNumber(this.sellPrice.Text, false))
            {
                DisplayAlert("Error", $"{this.sellPrice.Placeholder} should be number!", "OK");
                return false;
            }
            
            if (!Utils.IsNumber(this.stockQty.Text))
            {
                DisplayAlert("Error", $"{this.stockQty.Placeholder} should be number!", "OK");
                return false;
            }

            return true;
        }

        private void ClearControls()
        {
            _item = null;
            this.itemImage.Source = Utils.GetDefaultImage();
            this.itemName.Text = "";
            this.sellPrice.Text = null;
            this.description.Text = null;
            this.purchasePrice.Text = null;
            this.stockQty.Text = null;
        }

        private void OnBack(object sender, EventArgs e)
        {
            var backButton = sender as Button;
            backButton.IsEnabled = false;
            Navigation.PopAsync();
        }

        private void SetPageData(Item item)
        {
            if(_item != null)
            {
                this.itemName.Text = _item.ItemName;
                this.sellPrice.Text = Utils.ToString(_item.SellPrice);
                this.description.Text = Utils.ToString(_item.Description);
                this.purchasePrice.Text = Utils.ToString(_item.PurchasePrice);
                this.stockQty.Text = Utils.ToString(_item.StockQty);                
            }
            else
            {
                _item = new Item();
            }

            this.itemImage.Source = (_item == null || string.IsNullOrWhiteSpace(_item.ImagePath)) ? Utils.GetDefaultImage() : ImageSource.FromFile(_item.ImagePath);
        }

        private Item GetPageData()
        {
            _item = _item ?? new Item();

            _item.ItemName = this.itemName.Text;
            _item.SellPrice = Utils.ToDecimal(this.sellPrice.Text);
            _item.Description = Utils.ToString(this.description.Text);
            _item.PurchasePrice = Utils.ToDecimal(this.purchasePrice.Text);
            _item.StockQty = Utils.ToDecimal(this.stockQty.Text);           

            return _item;
        }

        
    }
}