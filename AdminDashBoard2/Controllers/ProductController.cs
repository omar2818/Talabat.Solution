using AdminDashboardWorkshop.Helpers;
using AdminDashboardWorkshop.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace AdminDashboardWorkshop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _unitOfWork.Repository<Product>().GetAllAsync();
            var mappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(products);
            
            return View(mappedProducts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.Image != null)
                {
                    model.PictureUrl = PictureSetting.UploadFile(model.Image, "products");
                }
                else
                {
                    model.PictureUrl = "images/products/hat-react2.png";
                }
                var mappedProduct = _mapper.Map<ProductViewModel, Product>(model);

                await _unitOfWork.Repository<Product>().AddAsync(mappedProduct);
                await _unitOfWork.Complete();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetAsync(id);

            var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);

            return View(mappedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductViewModel model)
        {
            if(id != model.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    if(model.PictureUrl != null)
                    {
                        PictureSetting.DeleteFile(model.PictureUrl, "products");
                    }

                    model.PictureUrl = PictureSetting.UploadFile(model.Image, "products");
                    var mappedProduct = _mapper.Map<ProductViewModel, Product>(model);

                    _unitOfWork.Repository<Product>().Update(mappedProduct);
                    var Result = await _unitOfWork.Complete();

                    if(Result > 0)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var Product = await _unitOfWork.Repository<Product>().GetAsync(id);

            var mappedProduct = _mapper.Map<Product, ProductViewModel>(Product);

            return View(mappedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, ProductViewModel model)
        {
			if (id != model.Id)
			{
				return NotFound();
			}

            try
            {
                var product = await _unitOfWork.Repository<Product>().GetAsync(id);

                if (product.PictureUrl != null)
                {
                    PictureSetting.DeleteFile(product.PictureUrl, "products");
                }

                _unitOfWork.Repository<Product>().Delete(product);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(model);
            }
		}
    }
}
