using CourseProject.Helpers;
using CourseProject.Models;
using CourseProject.Models.Enums;
using CourseProject.Models.ViewModels;
using CourseProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourseProject.Services;
using Markdig;

namespace CourseProject.Controllers
{
    [Authorize]
    public class CollectionController : Controller
    {
        private IAccountService _accountService;
        private IUnitOfWork _unitOfWork;

        public CollectionController(IAccountService accountService, IUnitOfWork unitOfWork)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<IActionResult> UserCollections(string? userId = null)
        {
            
            if(userId == null)
            {
                userId = await _accountService.GetUserIdAsync(User);
            }
            var collections = (await _unitOfWork.CollectionRepository.GetByUserAsync(userId)).ToList();
            if (collections != null && collections[0] != null)
            {
                var isOwner = collections[0].UserId == await _accountService.GetUserIdAsync(User);
                var isAdmin = User.IsInRole("Admin");
                ViewBag.IsOwnerOrAdmin = isOwner || isAdmin;
            }

            collections.Reverse();
            foreach(var col in collections)
            {
                col.Description = Markdown.ToHtml(col.Description);
            }

            return View(collections);
        }



        [HttpGet]
        public IActionResult CreateCollection()
        {
            ViewBag.Theme = EnumConverter.GetCollectionThemes();
            ViewBag.PropertyType = EnumConverter.GetPropertyTypes();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCollection(CreateCollectionViewModel model, string[] propertyNames, string[] propertyTypes)
        {
            var imageName = DateTime.Now.ToString().Replace(".","-") + model.Image.FileName;
            var split = model.Image.FileName.Split(".");
            var mime = "image/" + split[split.Length - 1];
            
            var path = await MegaImageWrite(model.Image, imageName);

            var userId = await _accountService.GetUserIdAsync(User);
            var collection = new Collection()
            {
                Name = model.Name,
                Description = model.Description,
                Theme = model.Theme,
                UserId = userId,
                ImagePath = path,
                ImageMime = mime
            };
            await _unitOfWork.CollectionRepository.CreateAsync(collection);


            List<CollectionProperty> properties = new();
            for(int i=0;i<propertyNames.Length;i++)
            { 
                var propertyName = propertyNames[i]; 
                var propertyType = EnumConverter.ParseEnum<PropertyType>(propertyTypes[i]);

                var collectionProperty = new CollectionProperty()
                {
                    Name = propertyName,
                    Type = propertyType,
                    CollectionId = collection.Id,
                };
                properties.Add(collectionProperty);
            }
            await _unitOfWork.CollectionPropertyRepository.CreateRangeAsync(properties);

            return RedirectToAction("UserCollections", "Collection");
        }

        public async Task<IActionResult> DeleteCollection(Guid collectionId)
        {
            var collection = await _unitOfWork.CollectionRepository.GetAsync(collectionId);
            var items = await _unitOfWork.ItemRepository.GetByCollectionAsync(collection.Id);

            await _unitOfWork.ItemRepository.DeleteRangeAsync(items);
            
            await _unitOfWork.CollectionRepository.DeleteAsync(collection);

            return RedirectToAction("UserCollections","Collection");
        }




        public async Task<FileContentResult> MegaImageRead(Guid collectionId)
        {
            var collection = await _unitOfWork.CollectionRepository.GetAsync(collectionId);

            var bytes = await MegaService.ReadImageAsync(collection.ImagePath);
            return File(bytes, collection.ImageMime);
        }


        public async Task<string> MegaImageWrite(IFormFile image, string imageName)
        {
            return await MegaService.UploadImageAsync(image, imageName);
        }

    }
}
