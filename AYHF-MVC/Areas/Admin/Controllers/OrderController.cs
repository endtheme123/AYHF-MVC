using AYHF.DataAccess.Repository;
using AYHF.DataAccess.Repository.IRepository;
using AYHF.Models;
using AYHF.Models.ViewModels;
using AYHF.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AYHF_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            OrderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeaderRepo.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetailRepo.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product")
            };
            return View(OrderVM);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult UpdateOrderDetail()
        {
            var orderHeader = _unitOfWork.OrderHeaderRepo.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.Name = OrderVM.OrderHeader.Name;
            orderHeader.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeader.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeader.City = OrderVM.OrderHeader.City;
            orderHeader.State = OrderVM.OrderHeader.State;
            orderHeader.PostalCode = OrderVM.OrderHeader.PostalCode;

            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
            {
                orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
            }
            if(!string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
            {
                orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            }

            _unitOfWork.OrderHeaderRepo.Update(orderHeader);
            _unitOfWork.Save();
            TempData["success"] = "Order Detail updated successfully";

            return RedirectToAction(nameof(Details), new {orderId = orderHeader.Id});
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Shipper)]
        public IActionResult StartPackaging()
        {
            
            _unitOfWork.OrderHeaderRepo.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusPackaging);
            _unitOfWork.Save();
            TempData["success"] = "Order start packaging...";

            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Shipper)]
        public IActionResult DonePackaging()
        {

            _unitOfWork.OrderHeaderRepo.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusPacked);
            _unitOfWork.Save();
            TempData["success"] = "Order packed";

            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Shipper)]
        public IActionResult ShipOrder()
        {
            var orderHeader = _unitOfWork.OrderHeaderRepo.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
            orderHeader.OrderStatus = SD.StatusShipping;
            orderHeader.ShippingDate = DateTime.Now;
            _unitOfWork.OrderHeaderRepo.Update(orderHeader);
            _unitOfWork.Save();
            TempData["success"] = "Order start shipping...";

            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Shipper)]
        public IActionResult ShipConfirm()
        {

            _unitOfWork.OrderHeaderRepo.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusShipped);
            _unitOfWork.Save();
            TempData["success"] = "Order shipped";

            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Customer)]
        public IActionResult CancelOrder()
        {

            _unitOfWork.OrderHeaderRepo.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusCancelled);
            _unitOfWork.Save();
            TempData["success"] = "Order Cancelled";

            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> obj;

            if (User.IsInRole(SD.Role_Admin))
            {
                obj = _unitOfWork.OrderHeaderRepo.GetAll(includeProperties: "ApplicationUser").ToList();
            
            } else if (User.IsInRole(SD.Role_Shipper))
            {
                obj = _unitOfWork.OrderHeaderRepo.GetAll(
                    u => (u.OrderStatus == SD.StatusShipping 
                    || u.OrderStatus == SD.StatusPackaging
                    || u.OrderStatus == SD.StatusPacked
                    || u.OrderStatus == SD.StatusApproved) 
                    && u.PaymentStatus == SD.PaymentStatusApproved, includeProperties: "ApplicationUser").ToList();

            } else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                obj = _unitOfWork.OrderHeaderRepo.GetAll(u=>u.ApplicationUserId == claim, includeProperties:"ApplicationUser");
            }

            switch (status)
            {
                case "pending":
                    obj = obj.Where(u => u.PaymentStatus == SD.PaymentStatusPending);
                    break;
                case "completed":
                    obj = obj.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    obj = obj.Where(u => u.OrderStatus == SD.StatusApproved);

                    break;
                case "shipping":
                    obj = obj.Where(u => u.OrderStatus == SD.StatusShipping);

                    break;
                case "packaging":
                    obj = obj.Where(u => u.OrderStatus == SD.StatusPackaging);

                    break;
                case "packed":
                    obj = obj.Where(u => u.OrderStatus == SD.StatusPacked);

                    break;
                case "all":
                    
                    break;
            }

            return Json(new { data = obj });
        }

        
        #endregion
    }

}
