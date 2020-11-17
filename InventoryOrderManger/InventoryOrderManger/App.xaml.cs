using System;
using InventoryOrderManger.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InventoryOrderManger
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new SelectionPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
