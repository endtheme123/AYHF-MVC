using AYHF.DataAccess.Data;
using AYHF.Models;
using Microsoft.AspNetCore.Mvc;
//using AYHF.DataAccess.Repository;
using AYHF.DataAccess.Repository.IRepository;
using AYHF.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using AYHF.Utility;

namespace AYHF_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.ProductRepo.GetAll().ToList();
            return View(objProductList);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product()
            };
            if(id==null || id == 0)
            {
                return View(productVM);
            } else
            {
                productVM.Product = _unitOfWork.ProductRepo.GetFirstOrDefault(u => u.Id == id);
                return View(productVM);
            }
            
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            //if (obj.Name.ToLower() == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("Name", "The display order and name should be different");
            //}
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\Product");
                    if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = @"\Images\Product\" + fileName;
                }
                if (obj.Product.Id == 0)
                {
                    _unitOfWork.ProductRepo.Add(obj.Product);
                } else
                {
                    _unitOfWork.ProductRepo.Update(obj.Product);
                }
                
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            } else
            {
                ProductVM productVM = new ProductVM()
                {
                    Product = new Product()
                };
                return View(productVM);
            }

            

        }
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Product product = _unitOfWork.ProductRepo.GetFirstOrDefault(u => u.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(product);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    //if (obj.Name.ToLower() == obj.DisplayOrder.ToString())
        //    //{
        //    //    ModelState.AddModelError("Name", "The display order and name should be different");
        //    //}
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.ProductRepo.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product created successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();

        //}

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Product product = _unitOfWork.ProductRepo.GetFirstOrDefault(u => u.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(product);
        //}

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? obj = _unitOfWork.ProductRepo.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.ProductRepo.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product created successfully";
            return RedirectToAction("Index");

        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.ProductRepo.GetAll().ToList();
            return Json(new {data = objProductList});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Product? obj = _unitOfWork.ProductRepo.GetFirstOrDefault(u => u.Id == id);
            if(obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.ProductRepo.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "delete successlly" });
        }
        #endregion
    }
}
