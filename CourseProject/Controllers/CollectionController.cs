using CourseProject.Helpers;
using CourseProject.Models;
using CourseProject.Models.Enums;
using CourseProject.Models.ViewModels;
using CourseProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Dropbox.Api;
using Dropbox.Api.Files;

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
            //ViewBag.Image = await Drop();



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
            await DropImageWrite(model.Image);
            var user = await _accountService.GetUserAsync(User);
            var userId = await _accountService.GetUserIdAsync(User);
            var collection = new Collection()
            {
                Name = model.Name,
                Description = model.Description,
                Theme = model.Theme,
                User = user,
                UserId = userId,
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
        [HttpGet]
        public async Task<IActionResult> CollectionItems(Guid collectionId)
        {
            var items = await _itemRepository.GetByCollectionAsync(collectionId);

            foreach(var item in items)
            {
                var properties = await _propertyRepository.GetByItemAsync(item.Id);
                item.Properties = new List<Property>(properties);
            }
            var collection = await _collectionRepository.GetAsync(collectionId);
            var collectionProperty = await _collectionPropertyRepository.GetByCollectionAsync(collection.Id);
            collection.Properties = collectionProperty.ToList();

            var tuple = new Tuple<List<Item>, Collection>(items.ToList(), collection);

            return View(tuple);
        }
        [HttpGet]
        public async Task<IActionResult> CreateItem(Guid collectionId)
        {

            var collection = await _collectionRepository.GetAsync(collectionId);
            var properties = await _collectionPropertyRepository.GetByCollectionAsync(collectionId);


            ViewBag.Collection = collection;
            ViewBag.Properties = properties;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(CreateItemViewModel model, string[] values)
        {
            var item = new Item()
            {
                Name = model.Name,
                CollectionId = model.CollectionId,
            };
            await _itemRepository.CreateAsync(item);

            var collectionPropertiesAsync = await _collectionPropertyRepository.GetByCollectionAsync(model.CollectionId);
            var collectionProperties = collectionPropertiesAsync.ToArray();
            var properties = new List<Property>();

            for (int i = 0; i < values.Length; i++) 
            {
                var property = new Property()
                {
                    ItemId = item.Id,
                    CollectionPropertyId = collectionProperties[i].Id,
                    PropertyValue = values[i]
                };
                properties.Add(property);
            }

            await _propertyRepository.CreateRangeAsync(properties);

            return RedirectToAction("UserCollections", "Collection");
        }

        public async Task<FileContentResult> DropImageRead(Guid collectionId)
        {
            var collection = await _collectionRepository.GetAsync(collectionId);
            string imageName = collection.ImagePath;

            string mimeType = "image/" + imageName.Split(".")[1];
            using (var dbx = new DropboxClient("sl.BNis_ZaoqUYvA-nI-AdqKtUH2pgZqlTD5eQKNJm9QBF5i7Qyd_sOeOhXTwnW74p2o6rtO9MMZaCICzHKLmtJtQhUPqvWcQFxSG434SoJl_7qnfJHiCiFeLWXi238TXC0qPZMMZyFqZuK"))
            {           
                using (var response = await dbx.Files.DownloadAsync(imageName))
                {
                    var bytes = await response.GetContentAsByteArrayAsync();
                    return File(bytes, mimeType);
                }
            }
        }
        public async Task DropImageWrite(IFormFile image)
        {
            
            string imageName = "/" + image.FileName;
            using (var dbx = new DropboxClient("sl.BNis_ZaoqUYvA-nI-AdqKtUH2pgZqlTD5eQKNJm9QBF5i7Qyd_sOeOhXTwnW74p2o6rtO9MMZaCICzHKLmtJtQhUPqvWcQFxSG434SoJl_7qnfJHiCiFeLWXi238TXC0qPZMMZyFqZuK"))
            {
                var response = await dbx.Files.UploadAsync(imageName, body:image.OpenReadStream());
            }
        }

    }
}
