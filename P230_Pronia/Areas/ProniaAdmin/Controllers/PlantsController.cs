using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P230_Pronia.DAL;
using P230_Pronia.Entities;
using P230_Pronia.Utilities.Extensions;
using P230_Pronia.ViewModels;

namespace P230_Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class PlantsController : Controller
    {
        private readonly ProniaDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PlantsController(ProniaDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            IEnumerable<Plant> model = _context.Plants.Include(p=>p.PlantImages).AsNoTracking().AsEnumerable();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Informations = _context.PlantDeliveryInformation.AsEnumerable();
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Tags = _context.Tags.AsEnumerable();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlantVM newPlant)
        {
            ViewBag.Informations = _context.PlantDeliveryInformation.AsEnumerable();
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Tags = _context.Tags.AsEnumerable();
            TempData["InvalidImages"] = string.Empty;
            if (!ModelState.IsValid)
            {
                return View();
            }
            if(!newPlant.HoverPhoto.IsValidFile("image/") || !newPlant.MainPhoto.IsValidFile("image/"))
            {
                ModelState.AddModelError(string.Empty, "Please choose image file");
                return View();
            }
            if (!newPlant.HoverPhoto.IsValidLength(1) || !newPlant.MainPhoto.IsValidLength(1))
            {
                ModelState.AddModelError(string.Empty, "Please choose image which size is maximum 1MB");
                return View();
            }

            Plant plant = new()
            {
                Name = newPlant.Name,
                Desc = newPlant.Desc,
                Price = newPlant.Price,
                SKU = newPlant.SKU,
                PlantDeliveryInformationId = newPlant.PlantDeliveryInformationId
            };
            string imageFolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            foreach (var image in newPlant.Images)
            {
                if(!image.IsValidFile("image/") || !image.IsValidLength(1))
                {
                    TempData["InvalidImages"] += image.FileName;
                    continue;
                }
                PlantImage plantImage = new()
                {
                    IsMain = false,
                    Path = await image.CreateImage(imageFolderPath,"website-images")
                };
                plant.PlantImages.Add(plantImage);
            }
            PlantImage main = new()
            {
                IsMain = true,
                Path = await newPlant.MainPhoto.CreateImage(imageFolderPath, "website-images")
            };
            plant.PlantImages.Add(main);
            PlantImage hover = new()
            {
                IsMain = null,
                Path = await newPlant.HoverPhoto.CreateImage(imageFolderPath, "website-images")
            };
            plant.PlantImages.Add(hover);

            foreach (int id in newPlant.CategoryIds)
            {
                PlantCategory category = new()
                {
                    CategoryId = id
                };
                plant.PlantCategories.Add(category);
            }
            foreach (int id in newPlant.TagIds)
            {
                PlantTag tag = new()
                {
                    TagId = id
                };
                plant.PlantTags.Add(tag);
            }
            _context.Plants.Add(plant);
            _context.SaveChanges();
            return RedirectToAction("Index", "Plants");
        }
    }
}
