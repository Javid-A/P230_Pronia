using Microsoft.AspNetCore.Identity;

namespace P230_Pronia.Entities
{
    public class User:IdentityUser
    {
        public string Fullname { get; set; }
        public ICollection<Basket>? Baskets { get; set; }
        public User()
        {
            Baskets = new List<Basket>();
        }
    }
}
