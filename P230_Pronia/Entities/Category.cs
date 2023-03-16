namespace P230_Pronia.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public List<PlantCategory> PlantCategories{ get; set; }
    }
}
