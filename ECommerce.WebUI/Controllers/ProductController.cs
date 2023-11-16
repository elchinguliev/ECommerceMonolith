using ECommerce.Business.Abstract;
using ECommerce.Entities.Models;
using ECommerce.WebUI.Models;
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

        public static bool FilterProcess { get; set; } = false;

        // GET: ProductController
        public async Task<ActionResult> Index(int page=1,int category=0 ,bool filterAz=false)
        {
            var products=await _productService.GetAllByCategory(category);
            products= _productService.GetAllByFilterAZ(products, filterAz);
            FilterProcess = !FilterProcess;


            int pageSize = 10;

            var model = new ProductListViewModel
            {
                CurrentFilterState= FilterProcess,
                Products = products.Skip((page-1)*pageSize).Take(pageSize).ToList(),
                CurrentCategory=category,
                PageCount=((int)Math.Ceiling(products.Count/(double)pageSize)),
                PageSize=pageSize,
                CurrentPage=page
            };
            return View(model);
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
