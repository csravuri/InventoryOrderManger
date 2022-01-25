using static InventoryOrderManger.Common.Enumerations;

namespace InventoryOrderManger.Models
{
    public class Sequence : BaseModel
    {
        public SequenceType SequenceType { get; set; }

        public int Count { get; set; }
    }
}