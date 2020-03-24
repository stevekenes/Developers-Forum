using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DevelopersForum.Models;
using DevelopersForum.ViewModels.Home;
using DevelopersForum.Models.Interfaces;
using System.Linq;
using DevelopersForum.ViewModels;

namespace DevelopersForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostService _postService;

        public HomeController(IPostService postService)
        {
            _postService = postService;
        }

        public IActionResult Index()
        {
            var model = BuildHomeIndexModel();
            return View(model);
        }

        private HomeIndexModel BuildHomeIndexModel()
        {
            var latestPosts = _postService.GetLatestPosts(10); 

            var posts = latestPosts.Select(post => new PostListingModel
            {
                Id = post.PostId,
                Title = post.Title,
                AuthorName = post.ApplicationUsers.UserName,
                AuthorId = post.ApplicationUsers.Id,
                AuthorRating = post.ApplicationUsers.Rating,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = GetForumListingForPost(post)
            });

            return new HomeIndexModel
            {
                LatestPosts = posts,
                SearchQuery = ""
            };
        }

        private ForumListingModel GetForumListingForPost(Post post)
        {
            var forum = post.Forum;

            return new ForumListingModel
            {
                Id = forum.ForumId,
                Name = forum.Title,
                ImageUrl = forum.ImageUrl
            };
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
