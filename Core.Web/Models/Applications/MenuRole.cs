
namespace MyWeb.Models
{
    public class MenuRole
    {
        public int MenuId { get; set; }
        public int RoleId { get; set; }
        public string InventoryItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}