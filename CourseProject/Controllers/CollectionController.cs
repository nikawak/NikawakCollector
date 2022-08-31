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

    public class CollectionController : Controller
    {
        private IConfiguration _configuration;
        private IAccountService _accountService;
        private IUnitOfWork _unitOfWork;

        public CollectionController(IAccountService accountService, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _accountService = accountService;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UserCollections(string? userId = null)
        {
            
            if(userId == null)
            {
                userId = await _accountService.GetUserIdAsync(User);
            }
            var collections = (await _unitOfWork.CollectionRepository.GetByUserAsync(userId));
            if (collections != null && collections.Any())
            {
                var isOwner = collections.ToList()[0].UserId == await _accountService.GetUserIdAsync(User);
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
        public async Task<IActionResult> UpdateCollection(Guid collectionId)
        {
            ViewBag.Theme = EnumConverter.GetCollectionThemes();
            ViewBag.PropertyType = EnumConverter.GetPropertyTypes();

            var collection = await _unitOfWork.CollectionRepository.GetAsync(collectionId);
            var collectionVM = new CollectionViewModel()
            {
                Id = collection.Id,
                Description = collection.Description,
                Theme = collection.Theme,
                Name = collection.Name
            };

            return View(collectionVM);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCollection(CollectionViewModel model, string[] propertyNames, string[] propertyTypes)
        {
            var defaultCollection = await _unitOfWork.CollectionRepository.GetAsync(model.Id);

            string mime = defaultCollection.ImageMime;
            string path = defaultCollection.ImagePath;
            if (model.Image != null)
            {
                var imageName = DateTime.Now.ToString().Replace(".", "-") + model.Image.FileName;
                var split = model.Image.FileName.Split(".");
                mime = "image/" + split[split.Length - 1];

                path = await MegaImageWrite(model.Image, imageName);
            }
            var userId = await _accountService.GetUserIdAsync(User);
            var collection = new Collection()
            {
                Id = model.Id,
                Name = model.Name,
                Properties = defaultCollection.Properties,
                Description = model.Description,
                Theme = model.Theme,
                UserId = userId,
                ImagePath = path,
                ImageMime = mime
            };

            await _unitOfWork.CollectionRepository.UpdateAsync(collection);

            return RedirectToAction("UserCollections");
        }
        [Authorize]
        [HttpGet]
        public IActionResult CreateCollection()
        {
            ViewBag.Theme = EnumConverter.GetCollectionThemes();
            ViewBag.PropertyType = EnumConverter.GetPropertyTypes();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCollection(CollectionViewModel model, string[] propertyNames, string[] propertyTypes)
        {

            var properties = CreateProperties(propertyNames, propertyTypes);

            string mime = "";
            string path = "";

            if (model.Image != null)
            {
                var imageName = DateTime.Now.ToString().Replace(".", "-") + model.Image.FileName;
                var split = model.Image.FileName.Split(".");
                mime = "image/" + split[split.Length - 1];

                path = await MegaImageWrite(model.Image, imageName);
            }
            
            var userId = await _accountService.GetUserIdAsync(User);
            var collection = new Collection()
            {
                Name = model.Name,
                Properties = properties,
                Description = model.Description,
                Theme = model.Theme,
                UserId = userId,
                ImagePath = path,
                ImageMime = mime
            };
            


            collection.Properties = properties;

            await _unitOfWork.CollectionPropertyRepository.CreateRangeAsync(properties);
            await _unitOfWork.CollectionRepository.CreateAsync(collection);


            return RedirectToAction("UserCollections", "Collection");
        }
        public List<CollectionProperty> CreateProperties(string[] propertyNames, string[] propertyTypes)
        {
            List<CollectionProperty> properties = new();
            for (int i = 0; i < propertyNames.Length; i++)
            {
                var propertyName = propertyNames[i];
                var propertyType = EnumConverter.ParseEnum<PropertyType>(propertyTypes[i]);

                var collectionProperty = new CollectionProperty()
                {
                    Name = propertyName,
                    Type = propertyType,
                };
                properties.Add(collectionProperty);
            }
            return properties;
        }

        public async Task<IActionResult> DeleteCollection(Guid collectionId)
        {
            var collection = await _unitOfWork.CollectionRepository.GetAsync(collectionId);
            var items = await _unitOfWork.ItemRepository.GetByCollectionAsync(collection.Id);

            await _unitOfWork.ItemRepository.DeleteRangeAsync(items);
            
            await _unitOfWork.CollectionRepository.DeleteAsync(collection);

            return RedirectToAction("UserCollections","Collection");
        }




        public async Task<FileResult> MegaImageRead(Guid collectionId)
        {
            var collection = await _unitOfWork.CollectionRepository.GetAsync(collectionId);
            if (string.IsNullOrEmpty(collection.ImagePath))
            {
                return File("~/img/tusk-4.jpg", "image/jpg");
            }

            var bytes = await MegaService.ReadImageAsync(collection.ImagePath); 
            
            return File(bytes, collection.ImageMime);
        }


        public async Task<string> MegaImageWrite(IFormFile image, string imageName)
        {
            return await MegaService.UploadImageAsync(image, imageName, _configuration);  
        }

    }
}
