using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevelopersForum.Interfaces;
using DevelopersForum.Models;

namespace DevelopersForum.Services
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ApplicationUsers> GetAll()
        {
            return _context.Users;
        }

        public ApplicationUsers GetById(string id)
        {
            return GetAll().FirstOrDefault(
                user => user.Id == id);
        }

        public Task IncrementRating(string id, Type type)
        {
            throw new NotImplementedException();
        }

        public async Task SetProfileImage(string id, Uri uri)
        {
            var user = GetById(id);
            user.ProfileImageUrl = uri.AbsoluteUri;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
