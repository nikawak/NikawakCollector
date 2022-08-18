using CourseProject.Helpers;
using CourseProject.Models;
using CourseProject.Models.Enums;
using CourseProject.Models.ViewModels;
using CourseProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourseProject.Services;

namespace CourseProject.Controllers
{
    [Authorize]
    public class CollectionController : Controller
    {
        private IAccountService _accountService;
        private ICollectionRepository _collectionRepository;
        private ICollectionPropertyRepository _collectionPropertyRepository;
        private IItemRepository _itemRepository;
        private IPropertyRepository _propertyRepository;

        public CollectionController(IAccountService accountService, ICollectionRepository collectionRepository, ICollectionPropertyRepository collectionPropertyRepository, IItemRepository itemRepository,  IPropertyRepository propertyRepository)
        {
            _accountService = accountService;
            _collectionRepository = collectionRepository;
            _collectionPropertyRepository = collectionPropertyRepository;
            _itemRepository = itemRepository;
            _propertyRepository = propertyRepository;
        }


        [HttpGet]
        public async Task<IActionResult> UserCollections()
        {
            var userId = await _accountService.GetUserIdAsync(User);
            var collections = await _collectionRepository.GetByUserAsync(userId);

            return View(collections.Reverse());
        }
        [HttpGet]
        public async Task<IActionResult> UserCollections(string userId)
        {
            var collections = (await _collectionRepository.GetByUserAsync(userId)).ToList();
            if (collections != null && collections[0] != null) 
            {
                var isOwner = collections[0].UserId == await _accountService.GetUserIdAsync(User);
                var isAdmin = User.IsInRole("Admin");
                ViewBag.IsOwnerOrAdmin = isOwner || isAdmin;
            }

            collections.Reverse();
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
            var imageName = "/" + DateTime.Now.ToString().Replace(".","-") + model.Image.FileName;
            await DropImageWrite(model.Image, imageName);
            var userId = await _accountService.GetUserIdAsync(User);
            var collection = new Collection()
            {
                Name = model.Name,
                Description = model.Description,
                Theme = model.Theme,
                UserId = userId,
                ImagePath = imageName
            };
            await _collectionRepository.CreateAsync(collection);


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
            await _collectionPropertyRepository.CreateRangeAsync(properties);

            return RedirectToAction("UserCollections", "Collection");
        }

        public async Task<IActionResult> DeleteCollection(Guid collectionId)
        {
            var collection = await _collectionRepository.GetAsync(collectionId);
            var items = await _itemRepository.GetByCollectionAsync(collection.Id);

            await _itemRepository.DeleteRangeAsync(items);
            
            await _collectionRepository.DeleteAsync(collection);

            return RedirectToAction("UserCollections","Collection");
        }


        public async Task<FileContentResult> DropImageRead(Guid collectionId)
        {
            var collection = await _collectionRepository.GetAsync(collectionId);
            string imageName = collection.ImagePath;

            var split = imageName.Split(".");
            string mimeType = "image/" + split[split.Length-1];

            var bytes = await DropBoxService.ReadImageAsync(imageName);
            return File(bytes, mimeType);
        }
        public async Task DropImageWrite(IFormFile image, string imageName)
        {
            await DropBoxService.UploadImageAsync(image, imageName);
        }

    }
}
