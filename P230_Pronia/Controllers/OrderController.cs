using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P230_Pronia.DAL;
using P230_Pronia.Entities;
using P230_Pronia.ViewModels;

namespace P230_Pronia.Controllers
{
    
    public class OrderController : Controller
    {
        private readonly ProniaDbContext _context;
        private readonly UserManager<User> _userManager;

        public OrderController(ProniaDbContext context,UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddBasket(int plantId, Plant basketPlant)
        {
            //return Json(plant.AddCart);
            if (User.Identity.IsAuthenticated)
            {
                PlantSizeColor? plant = _context.PlantSizeColors.FirstOrDefault(p => p.Id == plantId);
                if (plant is null) return NotFound();

                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                Basket? userActiveBasket = _context.Baskets
                                                .Include(b => b.User)
                                                   .FirstOrDefault(b => b.User.Id == user.Id && !b.IsOrdered) ?? new Basket();
                //BasketItem item = new BasketItem
                //{
                //    PlantSizeColorId = plant.Id,
                    //SizeId = basketPlant.AddCart.SizeId

                //}
                //userActiveBasket.BasketItems.Add()


                await Console.Out.WriteLineAsync(user.Baskets.Count.ToString());
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
