using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RattingSystem.Model;
using RattingSystem.Service.Interface;
using System.Security.Claims;

namespace RattingSystem.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly RattingSystemContext _rattingSystemContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserService _userService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnv;
        public SessionController(UserManager<IdentityUser> userManager,
            RattingSystemContext rattingSystemContext,
            IUserService userService,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnv)
        {
            _userManager = userManager;
            _rattingSystemContext = rattingSystemContext;
            _userService = userService;
            _hostingEnv = hostingEnv;
        }

        [HttpGet]
        //[Authorize(Roles = "user")]
        public IActionResult GetAllSession(string userId)
        {
            //var userId = _userService.GetUserId();
            List<Session> sesstionList = new List<Session>();
            var Rate = (from x in _rattingSystemContext.Rate where x.User_Id == userId select x.Session_Id);

            var Session = (from e in _rattingSystemContext.Session where !Rate.Contains(e.Id) select new { e.Id, e.SessionName, e.PresenterName}).ToList();
            return Ok(Session);
        }

        [HttpPost]
        //[Authorize(Roles = "user")]
        public IActionResult AddSession(Session session)
        {
            _rattingSystemContext.Session.Add(session);
            _rattingSystemContext.SaveChanges();
            return Ok();
        }
        [HttpPost]
        public IActionResult UploadSessionVideo(IFormFile sessionVideo)
        {
            string webRootPath = _hostingEnv.WebRootPath;
            string directory = Path.Combine(webRootPath, @"SessionsVideo");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string filePath = Path.Combine(directory, sessionVideo.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                sessionVideo.CopyTo(fileStream);

            }
            return Ok(filePath);
        }


    }
}
