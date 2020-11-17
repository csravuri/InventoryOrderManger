using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryOrderManger.Controllers
{
    public class SelectionController : BaseController
    {
        public void OnEvent(EventsController.Selection key)
        {
            try
            {
                switch (key)
                {
                    case EventsController.Selection.CreateItem:
                        OnCreateItem();
                        break;
                    case EventsController.Selection.ViewItem:
                        OnViewItem();
                        break;
                    case EventsController.Selection.CreateOrder:
                        OnCreateOrder();
                        break;
                    case EventsController.Selection.ViewOrder:
                        OnViewOrder();
                        break;
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                WriteError(ex);
            }

        }

        private void OnViewOrder()
        {
            throw new NotImplementedException();
        }

        private void OnCreateOrder()
        {
            throw new NotImplementedException();
        }

        private void OnViewItem()
        {
            throw new NotImplementedException();
        }

        private void OnCreateItem()
        {
            
        }
    }


}
