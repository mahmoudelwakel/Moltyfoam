using MasterMoltyfoam.Areas.Identity.Pages.Account;
using MasterMoltyfoam.Models;
using MasterMoltyfoam.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace MasterMoltyfoam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminstrationController : Controller
    {
      
        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        //public AdminstrationController(
        //    UserManager<IdentityUser> userManager,
        //    RoleManager<IdentityRole> roleManager)
        //{
        //    _userManager = userManager;
        //    _roleManager = roleManager;
        //}

        public IActionResult Index()
        {
            return View();
        }
        //[HttpGet]
        //public async Task<IActionResult> Edit(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    var userRoles = await _userManager.GetRolesAsync(user);
        //    var userClaims = await _userManager.GetClaimsAsync(user);
        //    EditUserViewModel model = new EditUserViewModel()
        //    {
        //        Id = user.Id,
        //        UserName = user.UserName,
        //       // FullName = user.FullName,
        //        Email = user.Email,
        //        Password = user.PasswordHash,
        //        PhoneNumber = user.PhoneNumber,
        //       // picture = user.Picture,
        //        Claims = userClaims.Select(x => x.Value).ToList(),
        //        Roles = userRoles

        //    };
        //    return View(model);
        //}
    }
}
