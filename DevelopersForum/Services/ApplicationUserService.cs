using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevelopersForum.Interfaces;
using DevelopersForum.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task UpdateUserRating(string id, Type type)
        {
            var user = GetById(id);
            user.Rating = CalculateUserRating(type, user.Rating);
            await _context.SaveChangesAsync();
        }

        private int CalculateUserRating(Type type, int userRating)
        {
            var inc = 0;

            if (type == typeof(Post))
                inc = 1;

            if (type == typeof(PostReply))
                inc = 3;

            return userRating + inc;
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
