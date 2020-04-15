using DevelopersForum.Models;
using DevelopersForum.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopersForum.Services
{
    public class ForumService : IForumService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUsers> _userManager;

        public ForumService(ApplicationDbContext context, UserManager<ApplicationUsers> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task Create(Forum forum)
        {
            _context.Add(forum);
            await _context.SaveChangesAsync();

        }

        public async Task Delete(int forumId)
        {
            var forum = GetById(forumId);
            _context.Remove(forum);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Forum> GetAll()
        {
            return _context.Forums
                .Include(f => f.Posts);
        }

        public Forum GetById(int id)
        {
            var forum = _context.Forums.Where(f => f.ForumId == id)
                .Include(f => f.Posts).ThenInclude(p => p.ApplicationUsers)
                .Include(f => f.Posts).ThenInclude(p => p.Replies).ThenInclude(r => r.ApplicationUsers)
                .FirstOrDefault();

            return forum;
        }
        public async Task<ICollection<Post>> GetAllPostByForumId(int id)
        {
            var post = _context.Posts.Include(c => c.ApplicationUsers).Where(c => c.Forum.ForumId == id).ToList();
            foreach (var item in post)
            {
                var user = await _userManager.FindByIdAsync(item.Id);
                item.ApplicationUsers = user;
                _context.Update(item);
                await _context.SaveChangesAsync();

            }
            return post;
        }

        public Task UpdateForumDescription(int forumId, string newDescription)
        {
            throw new NotImplementedException();
        }

        public Task UpdateForumTitle(int forumId, string newTitle)
        {
            throw new NotImplementedException();
        }

        public bool HasRecentPost(int forumId)
        {
            const int hoursAgo = 12;
            var window = DateTime.Now.AddHours(-hoursAgo);
            return GetById(forumId).Posts.Any(post => post.Created > window);
        }

        public IEnumerable<ApplicationUsers> GetActiveUsers(int forumId)
        {
            var post = GetById(forumId).Posts;

            if (post != null || !post.Any())
            {
                var postUsers = post.Select(p => p.ApplicationUsers);
                var replyUsers = post.SelectMany(p => p.Replies).Select(r => r.ApplicationUsers);
                return postUsers.Union(replyUsers).Distinct();
            }
            return new List<ApplicationUsers>();
        }
    }
}
