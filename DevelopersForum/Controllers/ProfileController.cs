using DevelopersForum.Interfaces;
using DevelopersForum.Models;
using DevelopersForum.ViewModels.Profile;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DevelopersForum.Controllers
{
    public class ProfileController : Controller
    {
        private const string V = "Admin";
        private readonly UserManager<ApplicationUsers> _useManager;
        private readonly IApplicationUser _userService;
        private readonly IUpload _uploadService;

        public ProfileController(UserManager<ApplicationUsers> useManager, 
            IApplicationUser userService,
            IUpload uploadService)
        {
            _useManager = useManager;
            _userService = userService;
            _uploadService = uploadService;
        }

        public IActionResult Detail(string id)
        {
            var user = _userService.GetById(id);
            var userRoles = _useManager.GetRolesAsync(user).Result;

            var model = new ProfileModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                UserRating = user.Rating.ToString(),
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                MemberSince = user.MemberSince,
                IsAdmin = userRoles.Contains("Admin")
            };
            return View(model);
        }
    }
}
