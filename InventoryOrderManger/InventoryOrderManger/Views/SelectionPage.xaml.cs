using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryOrderManger.Controllers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InventoryOrderManger.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectionPage : TabbedPage
    {
        SelectionController selectionController = null;
        public SelectionPage()
        {
            InitializeComponent();
            selectionController = new SelectionController();
        }

        private void OnCreateItem_Clicked(object sender, EventArgs e)
        {
            selectionController.OnEvent(EventsController.Selection.CreateItem);
        }

        private void OnCreateOrder_Clicked(object sender, EventArgs e)
        {

        }

        private void OnViewItem_Clicked(object sender, EventArgs e)
        {

        }

        private void OnViewOrder_Clicked(object sender, EventArgs e)
        {

        }
    }
}