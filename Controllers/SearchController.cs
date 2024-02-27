using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using vFlow.Models;
using vFlow.Repository.IRepository;

namespace vFlow.Controllers
{
    public class SearchController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SearchController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(string searchString,string searchType)
        {
            var matchedPosts = new List<Tuple<Post, int>>();
            if(searchString == null)
            {
                return View(matchedPosts);
            }
            var temp = new List<Post>();
            var posts = _unitOfWork.Posts.GetAll();
            var searchWords = searchString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                           .Select(word => word.ToUpper())
                                           .ToList();

            if (searchType == "Post")
            {
                foreach (var post in posts)
                {
                    var titleWords = post.Title.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                               .Select(word => word.ToUpper())
                                               .ToList();
                    var wordMatchCount = titleWords.Count(titleWord => searchWords.Contains(titleWord));
                    if (wordMatchCount > 0)
                    {
                        matchedPosts.Add(Tuple.Create(post, wordMatchCount));
                    }
                }
            }
            else
            {
                foreach (var post in posts)
                {
                    var tagMatchCount = post.Tags
                        .SelectMany(tag => tag.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                        .Select(tagWord => tagWord.ToUpper())
                        .Count(tagWord => searchWords.Contains(tagWord));

                    if (tagMatchCount > 0)
                    {
                        matchedPosts.Add(Tuple.Create(post, tagMatchCount));
                    }
                }
            }
            matchedPosts = matchedPosts.OrderByDescending(tuple => tuple.Item2).ToList();
            return View(matchedPosts);
            return Json(matchedPosts);
        }
    }
}
