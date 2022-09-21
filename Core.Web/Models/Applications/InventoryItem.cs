
namespace MyWeb.Models
{
    public class InventoryItem
    {
        public int InventoryItemId { get; set; }
        public string InventoryItemCode { get; set; }
        public string InventoryItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}