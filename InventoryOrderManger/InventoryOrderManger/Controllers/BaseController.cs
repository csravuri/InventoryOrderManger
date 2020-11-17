using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryOrderManger.Controllers
{
    public class BaseController
    {
        protected void WriteError(Exception ex)
        {
            //await DisplayAlert("No Camera", ":( No camera available.", "OK");
            //return;
        }
    }
}
