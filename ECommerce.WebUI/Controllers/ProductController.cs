using ECommerce.Business.Abstract;
using ECommerce.Entities.Models;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productService;


        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public static bool FilterState { get; set; } = false;
        public static bool FilterStateHigher { get; set; } = false;

        // GET: ProductController
        [Authorize]
        public async Task<ActionResult> Index(int page=1,int category=0 ,bool filterAZ=false ,bool filterHigher=false)
        {
            int pageSize = 10;
            var products = await _productService.GetAllByCategory(category);
            products= _productService.GetAllByFilterAZ(products, filterAZ);
            FilterState = !FilterState;
    
            if (filterHigher)
            {
                products = _productService.GetAllByFilterHigherToLower(products, FilterStateHigher);
                FilterStateHigher = !FilterStateHigher;
            }
            var model = new ProductListViewModel
            {
                CurrentFilterStateHigher = FilterStateHigher,
                CurrentFilterState = FilterState,
                Products = products.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                CurrentCategory = category,
                PageCount = (int)Math.Ceiling(products.Count / (double)pageSize),
                PageSize = pageSize,
                CurrentPage = page
            };
            return View(model);

        }
        public async Task<List<Product>> Search(string word)
        {
            var allProducts = await _productService.GetAll();

            if (allProducts != null && !string.IsNullOrEmpty(word))
            {
                var result = allProducts.Where(r => r.ProductName.ToLower().Contains(word.ToLower())).ToList();
                var model = new ProductListViewModel
                {
                    Products = result
                };
                return result;
            }
            return null;
        }






        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
