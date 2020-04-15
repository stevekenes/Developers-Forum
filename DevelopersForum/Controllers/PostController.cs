using DevelopersForum.Interfaces;
using DevelopersForum.Models;
using DevelopersForum.Models.Interfaces;
using DevelopersForum.ViewModels;
using DevelopersForum.ViewModels.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopersForum.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IForumService _forumService;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly IApplicationUser _userService;

        public PostController(IPostService postService,
            IForumService forumService,
            UserManager<ApplicationUsers> userManager,
            IApplicationUser userService)
        {
            _postService = postService;
            _forumService = forumService;
            _userManager = userManager;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            var post = _postService.GetById(id);
            var replies = BuildPostReplies(post.Replies);

            var model = new PostIndexModel
            {
                Id = post.PostId,
                Title = post.Title,
                AuthorId = post.ApplicationUsers.Id,
                AuthorName = post.ApplicationUsers.UserName,
                AuthorImageUrl = post.ApplicationUsers.ProfileImageUrl,
                AuthorRating = post.ApplicationUsers.Rating,
                Created = post.Created,
                PostContent = post.Content,
                Replies = replies, 
                ForumId = post.Forum.ForumId,
                ForumName = post.Forum.Title,
                IsAuthorAdmin = IsAuthorAdmin(post.ApplicationUsers)
            };

            return View(model);
        }

        [Authorize]
        public IActionResult Create(int id)
        {
            var forum = _forumService.GetById(id);

            var model = new NewPostModel
            {
                ForumName = forum.Title,
                ForumId = forum.ForumId,
                ForumImageUrl = forum.ImageUrl,
                AuthorName = User.Identity.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(NewPostModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;
            var post = BuildPost(model, user);

            await _postService.Add(post);

            await _userService.UpdateUserRating(userId, typeof(Post));

            return RedirectToAction("Index", "Post", new { id = post.PostId });
        }

        private bool IsAuthorAdmin(ApplicationUsers applicationUsers)
        {
            return _userManager.GetRolesAsync(applicationUsers).Result.Contains("Admin");
        }

        private Post BuildPost(NewPostModel model, ApplicationUsers user)
        {
            var forum = _forumService.GetById(model.ForumId);
            return new Post
            {
                Title = model.Title,
                Content = model.Content,
                Created = DateTime.Now,
                ApplicationUsers = user,
                Forum = forum
            };
        }

        private IEnumerable<PostReplyModel> BuildPostReplies(IEnumerable<PostReply> replies)
        {
            return replies.Select(reply => new PostReplyModel
            {
                Id = reply.PostReplyId,
                AuthorName = reply.ApplicationUsers.UserName,
                AuthorId = reply.ApplicationUsers.Id,
                AuthorImageUrl = reply.ApplicationUsers.ProfileImageUrl,
                AuthorRating = reply.ApplicationUsers.Rating,
                Created = reply.Created,
                ReplyContent = reply.Content,
                IsAuthorAdmin = IsAuthorAdmin(reply.ApplicationUsers)
            });
        }
    }
}
