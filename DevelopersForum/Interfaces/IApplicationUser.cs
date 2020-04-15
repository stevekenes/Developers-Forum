using DevelopersForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopersForum.Interfaces
{
    public interface IApplicationUser
    {
        ApplicationUsers GetById(string id);
        IEnumerable<ApplicationUsers> GetAll();

        Task SetProfileImage(string id, Uri uri);
        Task UpdateUserRating(string id, Type type);
    }
}
