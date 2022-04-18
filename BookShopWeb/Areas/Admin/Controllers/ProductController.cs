using BookShop.DataAccess;
using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models;
using BookShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShopWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IWebHostEnvironment _he;


        public ProductController(IUnitofWork unitOfWork, IWebHostEnvironment he)
        {
            _unitOfWork = unitOfWork;
            _he = he;
        }
        public IActionResult Index()
        {
            var objCoverTypeList = _unitOfWork.CoverType.GetAll();

            return View(objCoverTypeList);
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new() //tightly binded view 
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name, 
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };
            if (id == null || id == 0)
            {
                //Create Product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                //Update the Product
            }
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile image)
        {

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images/Products", Path.GetFileName(image.FileName));
                    image.CopyToAsync(new FileStream(name, FileMode.Create));
                    obj.Product.ImageUrl = "images/" + image.FileName;
                }
                //if (image == null)
                //{
                //    obj.Product.ImageUrl = "/Images/Products/noImage.PNG";
                //}
                //_unitOfWork.CoverType.Update(coverType);
                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();
                TempData["success"] = "Cover Type Updated Successfully!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var coverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

            if (coverTypeFromDbFirst == null)
            {
                return NotFound();
            }
            return View(coverTypeFromDbFirst);
        }

        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            var obj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.CoverType.Remove(obj);
            TempData["delete"] = "Cover Type Deleted Successfully!";
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}