using ECommerce.Business.Abstract;
using ECommerce.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartSessionService _cartSessionService;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public CartController(ICartSessionService cartSessionService, IProductService productService, ICartService cartService)
        {
            _cartSessionService = cartSessionService;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> AddToCart(int productId)
        {
            var productToBeAdded = await _productService.GetById(productId);
            var cart = _cartSessionService.GetCart();
            _cartService.AddToCart(cart, productToBeAdded);

            _cartSessionService.SetCart(cart);

            TempData.Add("message", String.Format("Your product, {0} was added successfully to cart", productToBeAdded.ProductName));

            return RedirectToAction("Index", "Product");
        }
    }
}
