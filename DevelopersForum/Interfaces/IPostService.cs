﻿using DevelopersForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace DevelopersForum.Models.Interfaces
{
    public interface IPostService
    {
        Post GetById(int id);
        IEnumerable<Post> GetAll();
        IEnumerable<Post> GetFilteredPosts(Forum forum, string searchQuery);
        IEnumerable<Post> GetFilteredPosts(string searchQuery);
        IEnumerable<Post> GetPostsByForum(int id);
        IEnumerable<Post> GetLatestPosts(int n);

        Task Add(Post post);
        Task Delete(int id);
        Task EditPostContent(int id, string newContent);

        Task AddReply(PostReply reply);
        
    }
}
