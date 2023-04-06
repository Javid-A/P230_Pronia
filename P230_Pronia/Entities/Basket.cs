namespace P230_Pronia.Entities
{
    public class Basket:BaseEntity
    {
        public decimal TotalPrice { get; set; }
        public User User { get; set; }
        public Order Order { get; set; }
        public bool IsOrdered { get; set; } = false;
        public ICollection<BasketItem> BasketItems { get; set; } = null!;

        public Basket()
        {
            BasketItems = new List<BasketItem>();
        }
    }
}
