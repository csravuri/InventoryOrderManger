using System;
using InventoryOrderManger.Common;
using InventoryOrderManger.Database;
using InventoryOrderManger.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InventoryOrderManger.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemPage : ContentPage
    {
        private readonly Enumerations.OperationType operationType;
        private Item item;
        private DbConnection dbConnection = DbConnection.GetDbConnection();

        public ItemPage()
        {
            InitializeComponent();
        }

        public ItemPage(Enumerations.OperationType operationType, Item item = null) : this()
        {
            this.operationType = operationType;
            this.item = item ?? new Item();
            SetPageData();
            SetControlVisibility(operationType);
        }

        private void SetControlVisibility(Enumerations.OperationType operationType)
        {
            switch (operationType)
            {
                case Enumerations.OperationType.Create:
                    this.Title = "Create Item";
                    break;

                case Enumerations.OperationType.Update:
                    this.btnClear.IsVisible = false;
                    this.Title = "Update Item";
                    break;

                case Enumerations.OperationType.ExpressCreate:
                    this.itemImage.IsVisible = false;
                    this.description.IsVisible = false;
                    this.purchasePrice.IsVisible = false;
                    this.stockQty.IsVisible = false;
                    this.Title = "Express Create Item";
                    break;

                default:
                    break;
            }
        }

        private async void CaptureImage(object sender, EventArgs e)
        {
            try
            {
                var file = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Take photo"
                });
                if (file == null)
                    return;

                item.ImagePath = file.FullPath;
                this.itemImage.Source = ImageSource.FromFile(file.FullPath);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Not Supported", fnsEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("No Permission", pEx.Message, "OK");
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
                GetPageData(item);

                if (item.ID != Guid.Empty)
                {
                    await dbConnection.UpdateRecord(item);
                    await DisplayAlert("Success", $"{item.ItemName} updated.", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await dbConnection.InsertRecord(item);
                    await DisplayAlert("Success", $"{item.ItemName} created.", "OK");
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

            return true;
        }

        private void ClearControls()
        {
            item = new Item();
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

        private void SetPageData()
        {
            this.itemName.Text = item.ItemName;
            this.sellPrice.Text = Utils.ToString(item.SellPrice);
            this.description.Text = Utils.ToString(item.Description);
            this.purchasePrice.Text = Utils.ToString(item.PurchasePrice);
            this.stockQty.Text = Utils.ToString(item.StockQty);

            this.itemImage.Source = string.IsNullOrWhiteSpace(item.ImagePath) ? Utils.GetDefaultImage() : ImageSource.FromFile(item.ImagePath);
        }

        private void GetPageData(Item item)
        {
            item.ItemName = this.itemName.Text;
            item.SellPrice = Utils.ToDecimal(this.sellPrice.Text);
            item.Description = Utils.ToString(this.description.Text);
            item.PurchasePrice = Utils.ToDecimal(this.purchasePrice.Text);
            item.StockQty = Utils.ToDecimal(this.stockQty.Text);
        }
    }
}