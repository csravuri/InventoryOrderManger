using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryOrderManger.Common;
using InventoryOrderManger.Controllers;
using InventoryOrderManger.Database;
using InventoryOrderManger.Models;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace InventoryOrderManger.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectionPage : ContentPage
    {
        private DbConnection dbConnection = DbConnection.GetDbConnection();
        private DbManage dbManage = new DbManage();
        public SelectionPage()
        {
            InitializeComponent();          
        }

        private void OnCreateItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ItemPage(Enumerations.OperationType.Create));
        }

        private void OnViewItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ItemSearchPage());
        }

        private void OnCreateOrder_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OrderPage(Enumerations.OperationType.Create));
        }
        
        private void OnViewOrder_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OrderSearchPage());
        }

        private async void OnBackup_Clicked(object sender, EventArgs e)
        {
            try
            {
                string filePath = await dbManage.ExportDbData();

                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Export IO Manager data",
                    File = new ShareFile(filePath),
                });
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnRestore_Clicked(object sender, EventArgs e)
{
            try
            {
                FileData file = await CrossFilePicker.Current.PickFile();

                if (file == null)
                    return;

                string filePath = file.FilePath;
                
                await dbManage.ImportDbData(filePath);                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}