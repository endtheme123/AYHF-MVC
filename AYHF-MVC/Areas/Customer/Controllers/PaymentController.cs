using AYHF.DataAccess.Repository.IRepository;
using AYHF.Models;
using AYHF.Models.ViewModels;
using AYHF.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AYHF_MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class PaymentController : Controller
	{
        private readonly IUnitOfWork _unitOfWork;
        
        public PaymentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {
            
            return View();
        }
        
        public IActionResult ConfirmPayment(int id)
		{
			OrderHeader orderHeader = _unitOfWork.OrderHeaderRepo.GetFirstOrDefault(x => x.Id == id);
			_unitOfWork.OrderHeaderRepo.UpdateStatus(id, SD.StatusApproved);
			
			List<ShoppingCart> carts = _unitOfWork.CartRepo.GetAll(u => u.ApplicationUserId==orderHeader.ApplicationUserId).ToList();
			_unitOfWork.CartRepo.RemoveRange(carts);
			_unitOfWork.Save();
			return View(id);
		}

		public IActionResult GenerateReceipt(int orderId)
		{
            OrderHeader orderHeader = _unitOfWork.OrderHeaderRepo.GetFirstOrDefault(x => x.Id == orderId);

            _unitOfWork.OrderHeaderRepo.UpdateStatus(orderId, SD.StatusApproved, SD.PaymentStatusApproved);

            List<OrderDetail> orderDetails = _unitOfWork.OrderDetailRepo.GetAll(u => u.OrderHeaderId ==  orderId, includeProperties:"Product").ToList();
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = orderHeader,
                OrderDetails = orderDetails
            };
            
            return View(orderVM);
		}

        public IActionResult Cancel(int orderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeaderRepo.GetFirstOrDefault(x => x.Id == orderId);

            _unitOfWork.OrderHeaderRepo.UpdateStatus(orderId, SD.StatusCancelled, SD.PaymentStatusRejected);

            return RedirectToAction("Index","Home");
        }


    }
}
