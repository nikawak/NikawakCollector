using CourseProject.Models;
using CourseProject.Models.ViewModels;
using CourseProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace CourseProject.Controllers
{
    public class ItemController : Controller
    {
        private IAccountService _accountService;
        private IUnitOfWork _unitOfWork;

        public ItemController(IAccountService accountService, IUnitOfWork unitOfWork)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<IActionResult> CreateItem(Guid collectionId)
        {

            var collection = await _unitOfWork.CollectionRepository.GetAsync(collectionId);
            var properties = await _unitOfWork.CollectionPropertyRepository.GetByCollectionAsync(collectionId);


            ViewBag.Collection = collection;
            ViewBag.Properties = properties;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(CreateItemViewModel model, string[] values)
        {
            var tags = ValidTags(model.Tags);
            var existsTags = await _unitOfWork.TagRepository.ExistsAsync(tags.ToList());
            var notExistsTags = await _unitOfWork.TagRepository.NotExistsAsync(tags);

            await _unitOfWork.TagRepository.CreateRangeAsync(notExistsTags);

            var item = new Item()
            {
                Name = model.Name,
                CollectionId = model.CollectionId,
                CreatingDate = DateTime.Now,
                Tags = existsTags.Concat(notExistsTags).ToList()
            };
            await _unitOfWork.ItemRepository.CreateAsync(item);

            var collectionPropertiesAsync = await _unitOfWork.CollectionPropertyRepository.GetByCollectionAsync(model.CollectionId);
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

            await _unitOfWork.PropertyRepository.CreateRangeAsync(properties);

            return RedirectToAction("UserCollections", "Collection");
        }



        public async Task<IActionResult> CollectionItems(Guid collectionId)
        {
            var items = await _unitOfWork.ItemRepository.GetByCollectionAsync(collectionId);
            var collection = await _unitOfWork.CollectionRepository.GetAsync(collectionId);

            var tuple = new Tuple<List<Item>, Collection>(items.ToList(), collection);

            var isOwner = collection.UserId == await _accountService.GetUserIdAsync(User);
            var isAdmin = User.IsInRole("Admin");
            ViewBag.IsOwnerOrAdmin = isOwner || isAdmin;

            return View(tuple);
        }

        public async Task<IActionResult> ShowItem(Guid itemId)
        {
            var item = await _unitOfWork.ItemRepository.GetAsync(itemId);
            var collection = await _unitOfWork.CollectionRepository.GetAsync(item.CollectionId);

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
