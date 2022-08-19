using CourseProject.Models;
using CourseProject.Services;
using CourseProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class ProfileController : Controller
    {
        private IAccountService _accountService;
        private ICollectionRepository _collectionRepository;
        private ICollectionPropertyRepository _collectionPropertyRepository;
        private IItemRepository _itemRepository;
        private IPropertyRepository _propertyRepository;

        public ProfileController(IAccountService accountService, ICollectionRepository collectionRepository, ICollectionPropertyRepository collectionPropertyRepository, IItemRepository itemRepository, IPropertyRepository propertyRepository)
        {
            _accountService = accountService;
            _collectionRepository = collectionRepository;
            _collectionPropertyRepository = collectionPropertyRepository;
            _itemRepository = itemRepository;
            _propertyRepository = propertyRepository;
        }
        [Route("")]
        public async Task<IActionResult> Main()
        {
            var collections = await _collectionRepository.GetAllAsync();
            var biggestCollections = collections.OrderByDescending(x => x.CollectionItems.Count);

            var items = await _itemRepository.GetAllAsync();
            var lastestItems = items.OrderByDescending(x => x.CreatingDate);

            var tuple = new Tuple<List<Collection>, List<Item>>(biggestCollections.ToList(), lastestItems.ToList());
            return View(tuple);
        }
    }
}
