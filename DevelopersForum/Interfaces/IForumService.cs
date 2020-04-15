using DevelopersForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopersForum.Models.Interfaces
{
    public interface IForumService
    {
        Forum GetById(int id);
        IEnumerable<Forum> GetAll();

        Task<ICollection<Post>> GetAllPostByForumId(int id);
        Task Create(Forum forum);
        Task Delete(int forumId);
        Task UpdateForumTitle(int forumId, string newTitle);
        Task UpdateForumDescription(int forumId, string newDescription);
        bool HasRecentPost(int forumId);
        IEnumerable<ApplicationUsers> GetActiveUsers(int forumId);
    }
}
