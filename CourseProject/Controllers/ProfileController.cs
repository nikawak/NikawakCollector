using CourseProject.Models;
using CourseProject.Services;
using CourseProject.Services.Interfaces;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class ProfileController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public ProfileController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
       

        [Route("")]
        public async Task<IActionResult> Main()
        {
            var collections = await _unitOfWork.CollectionRepository.GetAllAsync();
            var biggestCollections = collections.OrderByDescending(x => x.CollectionItems.Count).ToList();

            var items = await _unitOfWork.ItemRepository.GetAllAsync();
            var lastestItems = items.OrderByDescending(x => x.CreatingDate).ToList();
            foreach (var col in collections)
            {
                col.Description = Markdown.ToHtml(col.Description);
            }

            var tags = (await _unitOfWork.TagRepository.GetAllAsync()).Where(x=>x.Items.Count > 0).ToList();
            var tuple = new Tuple<List<Collection>, List<Item>, List<Tag>>(biggestCollections, lastestItems, tags);
            return View(tuple);
        }
    }
}
