namespace P230_Pronia.Entities
{
    public class BasketItem:BaseEntity
    {
        public decimal UnitPrice { get; set; }
        public int SaleQuantity { get; set; }
        public int PlantSizeColorId { get; set; }
        public PlantSizeColor PlantSizeColor { get; set; }
    }
}
