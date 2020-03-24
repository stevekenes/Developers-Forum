using DevelopersForum.Models;
using DevelopersForum.Models.Interfaces;
using DevelopersForum.ViewModels;
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

        public async Task<IActionResult> Topic(int id)
        {
            var forum = _forumService.GetById(id);
            var allPost = await _forumService.GetAllPostByForumId(id);

            //var postListings = allPost.Select(post => new PostListingModel
            //{
            //    Id = post.Id,
            //    AuthorId = post.User.Id,
            //    AuthorRating = post.User.Rating,
            //    Title = post.Title,
            //    DatePosted = post.Created.ToString(),
            //    RepliesCount = post.Replies.Count(),
            //    Forum = BuildForumListing(post)

            //});
            var postListing = new List<PostListingModel>();
            foreach (var post in allPost)
            {
                var postListModel = new PostListingModel
                {
                    Id = post.PostId,
                    AuthorId = post.Id,
                    AuthorName = post.ApplicationUsers.UserName,
                    AuthorRating = post.ApplicationUsers.Rating,
                    Title = post.Title,
                    DatePosted = post.Created.ToString(),
                    RepliesCount = post.Replies.Count(),
                    Forum = BuildForumListing(post)
                };
                postListing.Add(postListModel);
            }
            var model = new ForumTopicModel
            {
                Posts = postListing,
                Forum = BuildForumListing(forum)
            };

            return View(model);

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
