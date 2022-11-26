using BethanysPieShop.Models;
using BethanysPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly BethanysPieShopDbContext mvcBethanysPieShopDbContext;

        public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepository, BethanysPieShopDbContext mvcBethanysPieShopDbContext)
        {
            _pieRepository = pieRepository;
            _categoryRepository = categoryRepository;
            this.mvcBethanysPieShopDbContext = mvcBethanysPieShopDbContext;
        }


        //public IActionResult List()
        //{
        //    //ViewBag.CurrentCategory = "Cheese cakes";

        //    //return View(_pieRepository.AllPies);

        //    PieListViewModel piesListViewModel = new PieListViewModel(_pieRepository.AllPies, "All pies");
        //    return View(piesListViewModel);
        //}

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPieViewModel addPieRequest)
        {
            var pie = new Pie
            {
                Name = addPieRequest.Name,
                ShortDescription = addPieRequest.ShortDescription,
                LongDescription = addPieRequest.LongDescription,
                AllergyInformation = addPieRequest.AllergyInformation,
                Price = addPieRequest.Price,
                ImageUrl = addPieRequest.ImageUrl,
                ImageThumbnailUrl = addPieRequest.ImageThumbnailUrl,
                IsPieOfTheWeek = addPieRequest.IsPieOfTheWeek,
                InStock = addPieRequest.InStock,
                CategoryId = addPieRequest.CategoryId
            };
            await mvcBethanysPieShopDbContext.Pies.AddAsync(pie);
            await mvcBethanysPieShopDbContext.SaveChangesAsync();
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var pies = await mvcBethanysPieShopDbContext.Pies.ToListAsync();
            return View(pies);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int pieId)
        {
            var pie = await mvcBethanysPieShopDbContext.Pies.FindAsync(pieId);

            if (pie != null)
            {
                mvcBethanysPieShopDbContext.Pies.Remove(pie);
                await mvcBethanysPieShopDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public ViewResult List(string category)
        {
            IEnumerable<Pie> pies;
            string? currentCategory;

            if (string.IsNullOrEmpty(category))
            {
                pies = _pieRepository.AllPies.OrderBy(p => p.PieId);
                currentCategory = "All pies";
            }
            else
            {
                pies = _pieRepository.AllPies.Where(p => p.Category.CategoryName == category).OrderBy(p => p.PieId);
                currentCategory = _categoryRepository.AllCategories.FirstOrDefault(c => c.CategoryName == category)?.CategoryName;
            }

            return View(new PieListViewModel(pies, currentCategory));
        }

        public IActionResult Details(int id)
        {
            var pie = _pieRepository.GetPieById(id);
            if (pie == null)
            {
                return NotFound();
            }
            else
            {
                return View(pie);
            }
        }

        public IActionResult Search()
        {
            return View();
        }
    }
}
