using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using static InventoryOrderManger.Common.Enumerations;

namespace InventoryOrderManger.Models
{
    public class Sequence : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int SequenceID { get; set; }
        public SequenceType SequenceType { get; set; }
        public int Count { get; set; }
    }
}
