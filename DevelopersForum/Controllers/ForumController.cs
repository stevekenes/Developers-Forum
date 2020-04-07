using DevelopersForum.Models;
using DevelopersForum.Models.Interfaces;
using DevelopersForum.ViewModels;
using DevelopersForum.ViewModels.Forum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopersForum.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForumService _forumService;
        private readonly IPostService _postService;

        public ForumController(IForumService forumService, IPostService postService)
        {
            _forumService = forumService;
            _postService = postService;
        }

        public IActionResult Index()
        {
            var forums = _forumService.GetAll()
                .Select(forum => new ForumListingModel
                {
                    Id = forum.ForumId,
                    Name = forum.Title,
                    Descritpion = forum.Description
                });

            var model = new ForumIndexModel
            {
                ForumList = forums
            };

            return View(model);
        }

        public IActionResult Topic(int id, string searchQuery)
        {
            var forum = _forumService.GetById(id);
             var posts = new List<Post>();

            posts = _postService.GetFilteredPosts(forum, searchQuery).ToList();

            var postListings = posts.Select(post => new PostListingModel
            {
                Id = post.PostId,
                AuthorId = post.ApplicationUsers.Id,
                AuthorRating = post.ApplicationUsers.Rating,
                Title = post.Title,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = BuildForumListing(post)

            });
            
            var model = new ForumTopicModel
            {
                Posts = postListings,
                Forum = BuildForumListing(forum)
            };

            return View(model);

        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Forum forum)
        {
            if (!ModelState.IsValid)
            {
                return View(forum);
            }

            await _forumService.Create(forum);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Search(int id, string searchQuery)
        {
            return RedirectToAction("Topic", new { id, searchQuery });
        }

        private ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.Forum;
            return BuildForumListing(forum);
        }

        private ForumListingModel BuildForumListing(Forum forum)
        {
            return new ForumListingModel
            {
                Id = forum.ForumId,
                Name = forum.Title,
                Descritpion = forum.Description,
                ImageUrl = forum.ImageUrl
            };
        }
    }
}
