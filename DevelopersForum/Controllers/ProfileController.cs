using DevelopersForum.Interfaces;
using DevelopersForum.Models;
using DevelopersForum.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopersForum.Controllers
{
    public class ProfileController : Controller
    {
        private const string V = "Admin";
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly IApplicationUser _userService;
        private readonly IUpload _uploadService;

        public ProfileController(UserManager<ApplicationUsers> userManager, 
            IApplicationUser userService,
            IUpload uploadService)
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = uploadService;
        }

        [Authorize]
        public IActionResult Detail(string id)
        {
            var user = _userService.GetById(id);
            var userRoles = _userManager.GetRolesAsync(user).Result;

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

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userId =  _userManager.GetUserId(User);

            // Connect to an Azure Storage Account Container
            // Get Blob Container

            // Parse the content disposition response header
            // Grab the filename

            // Get a reference to a block blob
            // On that block blob, upload our file <...file uploaded to the cloud

            // Set the user's profile image to the URI
            // Redirect to the user's profile page

            return View();
        }

        public IActionResult Index()
        {
            var users = _userService.GetAll()
                 .Select(user => new ProfileModel
                 {
                     UserId = user.Id,
                     UserName = user.UserName,
                     Email = user.Email,
                     ProfileImageUrl = user.ProfileImageUrl,
                     MemberSince = user.MemberSince
                 });

            var model = new ProfileListModel
            {
                Profiles = users
            };

            return View(model);
        }
    }
}
