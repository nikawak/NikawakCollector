using CourseProject.Models;
using CourseProject.Models.ViewModels;
using CourseProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace CourseProject.Controllers
{
    public class ItemController : Controller
    {
        private ITagRepository _tagRepository;
        private IAccountService _accountService;
        private ICollectionRepository _collectionRepository;
        private ICollectionPropertyRepository _collectionPropertyRepository;
        private IItemRepository _itemRepository;
        private IPropertyRepository _propertyRepository;

        public ItemController(IAccountService accountService, ITagRepository tagRepository, ICollectionRepository collectionRepository, ICollectionPropertyRepository collectionPropertyRepository, IItemRepository itemRepository, IPropertyRepository propertyRepository)
        {
            _tagRepository = tagRepository;
            _accountService = accountService;
            _collectionRepository = collectionRepository;
            _collectionPropertyRepository = collectionPropertyRepository;
            _itemRepository = itemRepository;
            _propertyRepository = propertyRepository;
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
            var tags = ValidTags(model.Tags);
            await _tagRepository.CreateRangeAsync(tags);

            var item = new Item()
            {
                Name = model.Name,
                CollectionId = model.CollectionId,
                Tags = tags.ToList()
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



        public async Task<IActionResult> CollectionItems(Guid collectionId)
        {
            var items = await _itemRepository.GetByCollectionAsync(collectionId);
            var collection = await _collectionRepository.GetAsync(collectionId);

            var tuple = new Tuple<List<Item>, Collection>(items.ToList(), collection);

            var isOwner = collection.UserId == await _accountService.GetUserIdAsync(User);
            var isAdmin = User.IsInRole("Admin");
            ViewBag.IsOwnerOrAdmin = isOwner || isAdmin;

            return View(tuple);
        }

        public async Task<IActionResult> ShowItem(Guid itemId)
        {
            var item = await _itemRepository.GetAsync(itemId);
            var collection = await _collectionRepository.GetAsync(item.CollectionId);

            

            return View(item);
        }


        public IEnumerable<Tag> ValidTags(string tagsString)
        {
            var tags = tagsString.Split(" ");
            var regex = new Regex(@"^#[A-Za-zА-Яа-я_0-9]");
            foreach (var tag in tags)
            {
                if (regex.IsMatch(tag))
                    yield return new Tag()
                    {
                        Name = tag
                    };
            }
        }
    }
   
}
