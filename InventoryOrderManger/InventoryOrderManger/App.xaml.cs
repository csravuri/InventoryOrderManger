using InventoryOrderManger.Views;
using Xamarin.Forms;

namespace InventoryOrderManger
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new SelectionPage())
            {
                BarBackgroundColor = Color.FromHex("#c863ff")
            };
        }

        public string MyFolder { get; set; }

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