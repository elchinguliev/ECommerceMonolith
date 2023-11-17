using ECommerce.Business.Abstract;
using ECommerce.Entities.Concrete;
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

        public async Task< IActionResult> RemoveFromCart(int productId)
        {
            var productToBeRemoved = await _productService.GetById(productId);
            var cart = _cartSessionService.GetCart();
            _cartService.RemoveFromCart(cart, productId);
            _cartSessionService.SetCart(cart);
            TempData.Add("message", String.Format("Your product, {0} was removed successfully from cart", productToBeRemoved.ProductName));
            return RedirectToAction("Index","Product");
        }


        public async Task<IActionResult> List()
        {
            var cart=_cartSessionService.GetCart();
            var model = new CartListViewModel
            {
                Cart=cart
            };
            return View(model);
        }


        public IActionResult Remove(int productId)
        {
            var cart = _cartSessionService.GetCart();

            _cartService.RemoveFromCart(cart, productId);
            _cartSessionService.SetCart(cart);
            TempData.Add("message", "Your Product was removed successfully from cart");
            return RedirectToAction("List");
        }


        public IActionResult increase(int productId)
        {
            var cart = _cartSessionService.GetCart();
            var cartline=cart.CartLines.FirstOrDefault(c=>c.Product.ProductId==productId);
            if (cartline.Quantity<cartline.Product.UnitsInStock)
            {
                cartline.Quantity++;
                _cartSessionService.SetCart(cart);
                TempData.Add("message", "One item added");
            }
         
            return RedirectToAction("List");
        }

        public IActionResult decrease(int productId)
        {
            var cart = _cartSessionService.GetCart();
            var cartline = cart.CartLines.FirstOrDefault(c => c.Product.ProductId==productId);
            if (cartline.Quantity>1)
            {
                cartline.Quantity--;
                _cartSessionService.SetCart(cart);
                TempData.Add("message", "One item deleted");
            }

            return RedirectToAction("List");
        }


    }
}
