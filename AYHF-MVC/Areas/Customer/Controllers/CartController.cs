using AYHF.DataAccess.Repository;
using AYHF.DataAccess.Repository.IRepository;
using AYHF.Models;
using AYHF.Models.ViewModels;
using AYHF.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AYHF_MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM CartVM {  get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            CartVM = new()
            {
                ShoppingCartList = _unitOfWork.CartRepo.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader = new()
                
            };

            

            foreach(var cart in CartVM.ShoppingCartList)
            {
                CartVM.OrderHeader.OrderTotal += cart.Product.Price * cart.Count;
            }
            
            return View(CartVM);
        }

        public IActionResult GenerateInvoice()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            CartVM = new()
            {
                ShoppingCartList = _unitOfWork.CartRepo.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader = new()

            };

            CartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUserRepo.GetFirstOrDefault(u => u.Id == userId);
            CartVM.OrderHeader.PhoneNumber = CartVM.OrderHeader.ApplicationUser.PhoneNumber;
			CartVM.OrderHeader.StreetAddress = CartVM.OrderHeader.ApplicationUser.StreetAddress;
			CartVM.OrderHeader.City = CartVM.OrderHeader.ApplicationUser.City;
			CartVM.OrderHeader.State = CartVM.OrderHeader.ApplicationUser.State;
			CartVM.OrderHeader.PostalCode = CartVM.OrderHeader.ApplicationUser.PostalCode;
			CartVM.OrderHeader.Name = CartVM.OrderHeader.ApplicationUser.Name;



			foreach (var cart in CartVM.ShoppingCartList)
            {
                CartVM.OrderHeader.OrderTotal += cart.Product.Price * cart.Count;
            }
            return View(CartVM);
        }

        [HttpPost]
        [ActionName("GenerateInvoice")]
		public IActionResult GenerateInvoicePOST()
		{
            
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            CartVM.ShoppingCartList = _unitOfWork.CartRepo.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product");


            CartVM.OrderHeader.OrderDate = DateTime.Now;
            CartVM.OrderHeader.ApplicationUserId = userId;
			CartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUserRepo.GetFirstOrDefault(u => u.Id == userId);

            if(CartVM.ShoppingCartList.Count() == 0)
            {
                TempData["error"] = "No item in cart!";
                return RedirectToAction(nameof(Index));
            }

			foreach (var cart in CartVM.ShoppingCartList)
			{
				CartVM.OrderHeader.OrderTotal += cart.Product.Price * cart.Count;
			}

            CartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            CartVM.OrderHeader.OrderStatus = SD.StatusPending;
			
            _unitOfWork.OrderHeaderRepo.Add(CartVM.OrderHeader);
            _unitOfWork.Save();
            foreach (var cart in CartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = CartVM.OrderHeader.Id,
                    Count = cart.Count,
                };
                _unitOfWork.OrderDetailRepo.Add(orderDetail);
				_unitOfWork.Save();
			}





			
			return RedirectToAction("ConfirmPayment", "Payment", new { area = "Customer",id = CartVM.OrderHeader.Id });
		}


        
       
		public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.CartRepo.GetFirstOrDefault(u => u.Id == cartId);
            cart.Count += 1;
            _unitOfWork.CartRepo.Update(cart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.CartRepo.GetFirstOrDefault(u => u.Id == cartId);
            if(cart.Count <= 1)
            {
                _unitOfWork.CartRepo.Remove(cart);
            } 
            else
            {
                cart.Count -= 1;
                _unitOfWork.CartRepo.Update(cart);
            }
            
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.CartRepo.GetFirstOrDefault(u => u.Id == cartId);
            
            _unitOfWork.CartRepo.Remove(cart);
            

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
