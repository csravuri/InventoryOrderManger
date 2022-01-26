namespace InventoryOrderManger.Models
{
    public class Sequence : BaseModel
    {
        public string SequenceType { get; set; }

        public int Count { get; set; }
    }
}