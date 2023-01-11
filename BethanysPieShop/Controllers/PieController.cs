using BethanysPieShop.Models;
using BethanysPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShop.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepository)
        {
            _pieRepository = pieRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            AddPieViewModel vm = new AddPieViewModel()
            {
                Categories = _categoryRepository.AllCategories.ToList()
            };

            return View(vm);
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

            await _pieRepository.SavePieAsync(pie);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var pie = await _pieRepository.GetPieById(id);

            if (pie != null)
            {
                var viewModel = new AddPieViewModel()
                {
                    PieId = pie.PieId,
                    Name = pie.Name,
                    ShortDescription = pie.ShortDescription,
                    LongDescription = pie.LongDescription,
                    AllergyInformation = pie.AllergyInformation,
                    Price = pie.Price,
                    ImageUrl = pie.ImageUrl,
                    ImageThumbnailUrl = pie.ImageThumbnailUrl,
                    CategoryId = pie.CategoryId,
                    Categories = _categoryRepository.AllCategories.ToList(),
                    IsPieOfTheWeek = pie.IsPieOfTheWeek,
                    InStock = pie.InStock
                };
                return await Task.Run(() => View("Edit", viewModel));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddPieViewModel editPieReqest)
        {
            var pie = await _pieRepository.GetPieById(editPieReqest.PieId);

            if (pie != null)
            {
                pie.Name = editPieReqest.Name;
                pie.ShortDescription = editPieReqest.ShortDescription;
                pie.LongDescription = editPieReqest.LongDescription;
                pie.AllergyInformation = editPieReqest.AllergyInformation;
                pie.Price = editPieReqest.Price;
                pie.ImageUrl = editPieReqest.ImageUrl;
                pie.ImageThumbnailUrl = editPieReqest.ImageThumbnailUrl;
                pie.CategoryId = editPieReqest.CategoryId;
                pie.IsPieOfTheWeek = editPieReqest.IsPieOfTheWeek;
                pie.InStock = editPieReqest.InStock;

                await _pieRepository.UpdatePieAsync(pie);

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var pies = _pieRepository.AllPies.ToList();
            return View(pies);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int pieId)
        {
            await _pieRepository.DeletePie(pieId);

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

        public ViewResult AllPies(string category)
        {
            IEnumerable<Pie> pies;
            List<Category> categories = new List<Category>();
            string? currentCategory;

            if (string.IsNullOrEmpty(category))
            {
                pies = _pieRepository.AllPies.OrderBy(p => p.PieId);
                categories = _categoryRepository.AllCategories.ToList();
                currentCategory = "All pies";
            }
            else
            {
                pies = _pieRepository.AllPies.Where(p => p.Category.CategoryName == category).OrderBy(p => p.PieId);
                currentCategory = _categoryRepository.AllCategories.FirstOrDefault(c => c.CategoryName == category)?.CategoryName;
            }

            return View(new AllPiesViewModel(pies, categories, currentCategory));
        }

        public ViewResult SearchPie(int categoryId, string pieName)
        {
            var result = new List<Pie>();
            var category = _categoryRepository.GetCategoryById(categoryId);

            if (category == null && string.IsNullOrEmpty(pieName) && categoryId == 0)
            {
                result = _pieRepository.AllPies.ToList();
            }
            else
            {
                if (string.IsNullOrEmpty(pieName))
                {
                    result = _pieRepository.AllPies.Where(p => p.CategoryId == categoryId).ToList();
                }
                else
                {
                    if (!string.IsNullOrEmpty(pieName) && categoryId == 0)
                    {
                        result = _pieRepository.AllPies.Where(p => p.Name.ToLower().Contains(pieName.ToLower())).ToList();
                    }
                    else
                    {
                        result = _pieRepository.AllPies.ToList();
                    }
                }
            }

            var vm = new AllPiesViewModel(result, _categoryRepository.AllCategories.ToList());

            return View("AllPies", vm);
        }

        public IActionResult SearchQuery(string pieName)
        {
            List<Pie> result;

            if (pieName != "" && pieName != null)
            {
                result = _pieRepository.AllPies.Where(p => p.Name.Contains(pieName)).ToList();
            }
            else

                result = _pieRepository.AllPies.ToList();

            return View("AllPies");

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
