using DevelopersForum.Interfaces;
using DevelopersForum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevelopersForum.Controllers
{
    public class ProfileController : Controller
    {
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
            return View();
        }
    }
}
