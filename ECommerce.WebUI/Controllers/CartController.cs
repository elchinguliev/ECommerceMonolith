using ECommerce.Business.Abstract;
using ECommerce.Entities.Concrete;
using ECommerce.WebUI.Models;
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



        public async Task<IActionResult> AddToCart(int productId, int page, int category)
        {
            var productToBeAdded = await _productService.GetById(productId);
            var cart = _cartSessionService.GetCart();
            _cartService.AddToCart(cart, productToBeAdded);

            _cartSessionService.SetCart(cart);

            TempData.Add("message", String.Format("Your product, {0} was added successfully to cart", productToBeAdded.ProductName));

            return RedirectToAction("Index", "Product", new { page = page, category = category });
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


        public IActionResult Complete()
        {
            var shippingDetailsViewModel = new ShippingDetailsViewModel
            {
                ShippingDetails = new ShippingDetails()
            };
            return View(shippingDetailsViewModel);
        }

        [HttpPost]
        public IActionResult Complete(ShippingDetailsViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            TempData.Add("message", String.Format("Thank you {0} , your order is in progress.", data.ShippingDetails.Firstname));
            return View();
        }

    }
}
