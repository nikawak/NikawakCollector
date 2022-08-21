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
            var userId = await _accountService.GetUserIdAsync(User);

            var isOwner = collection.UserId == await _accountService.GetUserIdAsync(User);
            var isAdmin = User.IsInRole("Admin");
            ViewBag.IsOwnerOrAdmin = isOwner || isAdmin;

            var tuple = new Tuple<Item, Collection, string>(item, collection, userId);
            return View(tuple);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(Comment comment)
        {
            comment.CreationDate = DateTime.Now;
            await _unitOfWork.CommentRepository.CreateAsync(comment);

            return RedirectToAction("ShowItem", new { comment.ItemId });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var comment = await _unitOfWork.CommentRepository.GetAsync(commentId);
            var itemId = comment.ItemId;
            await _unitOfWork.CommentRepository.DeleteAsync(comment);

            return RedirectToAction("ShowItem", new { itemId });
        }

        public async Task<IActionResult> SearchItems(string searchString)
        {
            var strings = searchString.Replace("#", "").Split(" ");
            var tags = await _unitOfWork.TagRepository.GetAllAsync();
            var tagCount = new Dictionary<Tag, int>();

            foreach (var str in strings)
            {
                if (tags.Select(tag => tag.Name.Replace("#", "")).Contains(str))
                {
                    var tag = await _unitOfWork.TagRepository.GetByName("#"+str);
                    tagCount.Add(tag, tag.Items.Count);
                }
            }

            var orderedTags = tagCount.OrderByDescending(count => count.Value).Select(x=>x.Key).ToList();
            var items = new List<Item>();

            foreach(var tag in orderedTags)
            {
                items.AddRange(tag.Items);
            }
            return View(items);
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
