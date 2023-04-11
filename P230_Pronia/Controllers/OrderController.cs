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
            if (User.Identity.IsAuthenticated)
            {
                PlantSizeColor? plant = _context.PlantSizeColors.Include(p=>p.Plant).FirstOrDefault(p => p.PlantId == plantId && p.SizeId == basketPlant.AddCart.SizeId && p.ColorId == basketPlant.AddCart.ColorId);
                if (plant is null) return NotFound();

                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                Basket? userActiveBasket = _context.Baskets
                                                .Include(b => b.User)
                                                   .FirstOrDefault(b => b.User.Id == user.Id && !b.IsOrdered) ?? new Basket();

                BasketItem item = new()
                {
                    PlantSizeColorId = plant.Id,
                    SaleQuantity = basketPlant.AddCart.Quantity,
                    UnitPrice = plant.Plant.Price
                };
                userActiveBasket.BasketItems.Add(item);
                userActiveBasket.User = user;
                userActiveBasket.TotalPrice = userActiveBasket.BasketItems.Sum(p => p.SaleQuantity * p.UnitPrice);
                _context.Baskets.Add(userActiveBasket);
                _context.SaveChanges();
                 return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult GetSizes(int plantId,int colorId)
        {
            List<PlantSizeColor> plantSizeColor = _context.PlantSizeColors
                                                       .Include(p=>p.Size).Where(p => p.PlantId == plantId && p.ColorId == colorId).ToList();
            if (plantSizeColor is null) return Json(new { status = 404 });
            var sizes = plantSizeColor.Select(p => new { Id = p.SizeId, p.Size.Name }).ToList();
            return Json(sizes);
        }
    }
}
