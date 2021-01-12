using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryOrderManger.Common;
using InventoryOrderManger.Controllers;
using InventoryOrderManger.Database;
using InventoryOrderManger.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InventoryOrderManger.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectionPage : ContentPage
    {
        private DbConnection dbConnection = DbConnection.GetDbConnection();
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

        private void OnBackup_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Comming soon!", dbConnection.GetDBPath(), "OK");

        }

        private void OnRestore_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Comming soon!", dbConnection.GetDBPath(), "OK");            
        }
    }
}