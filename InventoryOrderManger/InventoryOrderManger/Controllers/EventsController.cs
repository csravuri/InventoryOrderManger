using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryOrderManger.Controllers
{
    public static class EventsController
    {
        public enum Selection
        {
            CreateItem,
            ViewItem,
            CreateOrder,
            ViewOrder
        }
    }
}
