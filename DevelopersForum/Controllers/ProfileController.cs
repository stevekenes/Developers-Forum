using DevelopersForum.Interfaces;
using DevelopersForum.Models;
using DevelopersForum.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DevelopersForum.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
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

        //[HttpPost]
        //public async Task<IActionResult> UploadProfileImage(IFormFile file)
        //{
        //    var userId =  _userManager.GetUserId(User);

        //    // Connect to an Azure Storage Account Container
        //    var connectionString = _configuration.GetConnectionString("AzureStorageAccount");
        //    // Get Blob Container
        //    var container = _uploadService.GetBlobContainer(connectionString, "profile-images");
        //    // Parse the content disposition response header
        //    var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
        //    // Grab the filename
        //    var filename = contentDisposition.FileName.Trim("""");
        //    // Get a reference to a block blob
        //    var blockBlob = container.GetBlobReference(fileName);
        //    // On that block blob, upload our file <...file uploaded to the cloud
        //    await blockBlob.UploadFormStreamAsync(file.OpenReadStream());
        //    // Set the user's profile image to the URI
        //    await _userService.SetProfileImage(userId, blockBlob.Uri);
        //    // Redirect to the user's profile page
        //    return RedirectToAction("Detail", "Profile", new { id = userId });
        //}

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var users = _userService.GetAll()
                .OrderByDescending(user => user.Rating)
                 .Select(user => new ProfileModel
                 {
                     UserName = user.UserName,
                     Email = user.Email,
                     ProfileImageUrl = user.ProfileImageUrl,
                     MemberSince = user.MemberSince,
                     UserRating = user.Rating.ToString()
                 });

            var model = new ProfileListModel
            {
                Profiles = users
            };

            return View(model);
        }
    }
}
