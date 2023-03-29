using Microsoft.AspNetCore.Mvc;
using NuGet.ContentModel;
using P230_Pronia.DAL;
using P230_Pronia.Entities;

namespace P230_Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class SliderController : Controller
    {
        private readonly ProniaDbContext _context;

        public SliderController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Slider> sliders = _context.Sliders.AsEnumerable();
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Slider newSlider)
        {
            if (newSlider.Image == null)
            {
                ModelState.AddModelError("Image", "Please choose image");
                return View();
            }
            if (!newSlider.Image.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "Please choose image type file");
                return View();
            }
            if ((double)newSlider.Image.Length / 1024 / 1024 > 1)
            {
                ModelState.AddModelError("Image", "Image size has to be maximum 1MB");
                return View();
            }
            var rootPath = @"C:\Users\Lenovo\source\repos\P230_Pronia\P230_Pronia\wwwroot";
            var folderPath = Path.Combine(rootPath, "assets", "images", "website-images");
            Random r = new();
            int random = r.Next(0, 1000);
            var fileName = string.Concat(random,newSlider.Image.FileName);
            var path = Path.Combine(folderPath, fileName);


            using (FileStream stream = new(path, FileMode.Create))
            {
                await newSlider.Image.CopyToAsync(stream);
            }
            newSlider.ImagePath = fileName;
            _context.Sliders.Add(newSlider);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
