namespace P230_Pronia.Entities
{
    public class Order:BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime DeliveredDate { get; set; }
        public  int BasketId { get; set; }
        public Basket Basket { get; set; }
    }
}
