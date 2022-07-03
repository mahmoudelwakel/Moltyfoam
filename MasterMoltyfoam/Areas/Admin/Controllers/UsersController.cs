using MasterMoltyfoam.Data;
using MasterMoltyfoam.Models;
using MasterMoltyfoam.Models.Utility;
using MasterMoltyfoam.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MasterMoltyfoam.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.ManagerUser)]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UsersController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            _userManager = userManager;
            _roleManager = roleManager;

        }
        public async Task<IActionResult> Index(int pg=1)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims=claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string UserId = claims.Value;
            var user= await db.ApplicationUsers.Where(m => m.Id != UserId).ToListAsync();
            const int pageSize = 2;
            if (pg < 1)
                pg = 1;
            int recsCount = user.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = user.Skip(resSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
           
           return View(data);
          
        }
        public async Task<IActionResult> LockUnLock(string ? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await db.ApplicationUsers.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddDays(100);
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);

            EditUserViewModel model = new EditUserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Password = user.PasswordHash ,
                PhoneNumber = user.PhoneNumber,
                picture = user.Picture,
                Claims = userClaims.Select(x => x.Value).ToList(),
                Roles = userRoles
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            //var userRoles = await _userManager.GetRolesAsync(user);
            //var oldRoleName = db.Roles.SingleOrDefault(r => r.Id == userRoles).Name;

            if (user == null)
            {
                return NotFound();
            }
         
            //var userClaims = await _userManager.GetClaimsAsync(user);
            else {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] picture = null;
                    var fs = files[0].OpenReadStream();
                    var ms = new MemoryStream();
                    fs.CopyTo(ms);
                    picture = ms.ToArray();
                    model.picture = picture;
                }

                user.UserName = model.UserName;
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.Picture = model.picture;
                user.PhoneNumber = model.PhoneNumber;
                string role = HttpContext.Request.Form["rdUserRole"].ToString();
                //if (oldRoleName != role)
                //{
                //   await _userManager.RemoveFromRoleAsync(user, oldRoleName);
                //   await _userManager.AddToRoleAsync(user, role);
                //}
                if (string.IsNullOrEmpty(role))
                {
                    await _userManager.AddToRoleAsync(user, SD.ClientUser);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
         
            return View(model);
        }
    }
}
