using CourseProject.Models;
using CourseProject.Models.ViewModels;
using CourseProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
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
            
            if (ModelState.IsValid)
            {
                foreach (var value in values)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        ModelState.AddModelError("", "You need to fill all item properties");

                        var col = await _unitOfWork.CollectionRepository.GetAsync(model.CollectionId);
                        var props = await _unitOfWork.CollectionPropertyRepository.GetByCollectionAsync(model.CollectionId);

                        ViewBag.Collection = col;
                        ViewBag.Properties = props;
                        return View(model);
                    }
                }

                var tags = ValidTags(model.Tags);
                var existsTags = await _unitOfWork.TagRepository.ExistsAsync(tags.ToList());
                var notExistsTags = await _unitOfWork.TagRepository.NotExistsAsync(tags);

                if (notExistsTags != null)
                    await _unitOfWork.TagRepository.CreateRangeAsync(notExistsTags);

                var item = new Item()
                {
                    Name = model.Name,
                    CollectionId = model.CollectionId,
                    CreatingDate = DateTime.Now,
                    IsPrivate = model.IsPrivate,
                    Tags = existsTags.Concat(notExistsTags).ToList()
                };
                await _unitOfWork.ItemRepository.CreateAsync(item);

                var collectionProperties = (await _unitOfWork.CollectionPropertyRepository.GetByCollectionAsync(model.CollectionId)).ToArray();
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
            }
            return RedirectToAction("CollectionItems", new { model.CollectionId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem(Guid itemId)
        {
            var item = await _unitOfWork.ItemRepository.GetAsync(itemId);
            var collectionId = item.CollectionId;
            await _unitOfWork.ItemRepository.DeleteAsync(item);

            return RedirectToAction("CollectionItems", new { collectionId });
        }

        public async Task<IActionResult> CollectionItems(Guid collectionId)
        {
            var items = await _unitOfWork.ItemRepository.GetByCollectionAsync(collectionId);
            var collection = await _unitOfWork.CollectionRepository.GetAsync(collectionId);

            if (User.Identity.IsAuthenticated)
            {
                var isOwnerOrAdmin = collection.UserId == await _accountService.GetUserIdAsync(User) || User.IsInRole("Admin");
                ViewBag.IsOwnerOrAdmin = isOwnerOrAdmin;
                if (isOwnerOrAdmin)
                {
                    items = await _unitOfWork.ItemRepository.GetByCollectionWithPrivateAsync(collectionId);
                }
            }
            else
            {
                ViewBag.IsOwnerOrAdmin = false;
            }

            var tuple = new Tuple<List<Item>, Collection>(items.ToList(), collection);
            return View(tuple);
        }

        public async Task<IActionResult> ShowItem(Guid itemId)
        {
            var item = await _unitOfWork.ItemRepository.GetAsync(itemId);
            var collection = await _unitOfWork.CollectionRepository.GetAsync(item.CollectionId);
            var userId = await _accountService.GetUserIdAsync(User);

            var isOwner = collection.UserId == userId;
            var isAdmin = User.IsInRole("Admin");
            ViewBag.IsOwnerOrAdmin = isOwner || isAdmin;

            var like = await _unitOfWork.LikeRepository.GetByUserAndItem(userId, itemId);
            ViewData["IsLiked"] = like != null;

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

        public async Task<IActionResult> ChangeLike(Guid itemId)
        {
            var userId = await _accountService.GetUserIdAsync(User);
            var like = await _unitOfWork.LikeRepository.GetByUserAndItem(userId, itemId);
            if (like == null)
            {
                like = new Like()
                {
                    ItemId = itemId,
                    SenderId = userId,
                };
                await _unitOfWork.LikeRepository.CreateAsync(like);
            }
            else
            {
                await _unitOfWork.LikeRepository.DeleteAsync(like);
            }

            return RedirectToAction("ShowItem", new { itemId });
        }
        
        public async Task<IActionResult> SearchItems(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return View(null);
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
            if (string.IsNullOrEmpty(tagsString)) yield return null;
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
