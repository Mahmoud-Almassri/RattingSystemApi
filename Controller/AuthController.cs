using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RattingSystem.Model;
using RattingSystem.Model.DTO;
using RattingSystem.Model.Common;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RattingSystem.Service.Interface;

namespace RattingSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RattingSystemContext _rattingSystemContext;
        private readonly IUserService _userService;
        AppConfiguration _appConfiguration = new AppConfiguration();
        public AuthController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RattingSystemContext rattingSystemContext,
            IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _rattingSystemContext = rattingSystemContext;
            _userService = userService;
        }
        [HttpPost("Login")]
        public async Task<LoginResponseDTO> Login(LoginDTO loginDTO)
        {
            IdentityUser user = _userManager.Users.FirstOrDefault(x => x.UserName == loginDTO.UserName);
            
            if (user != null)
            {
                Microsoft.AspNetCore.Identity.SignInResult oSignInResult = await _signInManager.PasswordSignInAsync(loginDTO.UserName, loginDTO.Password, false, false);
                if (oSignInResult.Succeeded)
                {
                    IList<string> roles = await _userManager.GetRolesAsync(user);
                    IList<Claim> claims = await BuildClaims(user);
                    LoginResponseDTO loginResponseDTO = new LoginResponseDTO();
                    loginResponseDTO.AccessToken = WriteToken(claims);
                    loginResponseDTO.UserId = new Guid(user.Id);
                    loginResponseDTO.UserName = user.UserName;
                    loginResponseDTO.Roles = roles;
                    
                    return loginResponseDTO;
                }
                else
                {
                    throw new ValidationException("Password Is Wrong");
                }

            }
            else
            {
                throw new ValidationException("UserName is Wrong");
            }
        }
        [HttpPost("Registration")]
        public async Task<IActionResult> Registration(RegistrationDTO registrationDTO)
        {
            if (_userManager.Users.Any(x => x.UserName == registrationDTO.UserName))
            {
                throw new ValidationException("UserName Already Exists");
            }
            IdentityUser identityUser = new IdentityUser();
            identityUser.UserName = registrationDTO.UserName;
            identityUser.Email = registrationDTO.Email;


            IdentityResult result = await _userManager.CreateAsync(identityUser, registrationDTO.Password);
            if (result.Succeeded)
            {
                var Role = _rattingSystemContext.Roles.Where(x => x.Name.ToLower().Equals("user")).FirstOrDefault();
                IdentityUserRole<string> userRole = new IdentityUserRole<string>();
                userRole.UserId = identityUser.Id;
                userRole.RoleId = Role.Id;
                _rattingSystemContext.UserRoles.Add(userRole);
                _rattingSystemContext.SaveChanges();
                return Ok();
            }
            else
            {
                throw new ValidationException("Error While Creating User");
            }

        }

        private async Task<IList<Claim>> BuildClaims(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, !string.IsNullOrEmpty(user.Id) ? user.Id : string.Empty),
                new Claim(ClaimTypes.Name, !string.IsNullOrEmpty(user.UserName) ? user.UserName : "")
            };
            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

            return claims;
        }
        private string WriteToken(IList<Claim> claims)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfiguration.JWTKey));

            JwtSecurityToken jwtToken = new JwtSecurityToken(
                    issuer: _appConfiguration.Issuer,
                    audience: _appConfiguration.Audience,
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddYears(100),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return token;
        }
    }
}
