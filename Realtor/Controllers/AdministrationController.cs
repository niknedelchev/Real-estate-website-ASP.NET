using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Realtor.Areas.Identity.Pages.Data;
using Realtor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdministrationController : Controller
    {

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateRole(CreateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null) 
            {
                ViewBag.ErrorMessage = $"Role with id {id} cannot be found.";
                return View("NotFound");
            }

            var model = new EditRoleModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                    
                }
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {model.Id} cannot be found.";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
           
        }
        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUserRoles(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);

            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {roleId} cannot be found.";
                return View("NotFound");
            }

            var model = new List<UserRolesModel>();

            foreach (var user in userManager.Users)
            {
                var userRolesModel = new UserRolesModel
                {
                    userId = user.Id,
                    userName = user.UserName
                };

            if (await userManager.IsInRoleAsync(user, role.Name))
            {
                    userRolesModel.isSelected = true;
                }
                else
                {
                    userRolesModel.isSelected = false;
                }
                model.Add(userRolesModel);
            }
         
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUserRoles(List<UserRolesModel> model, string roleId) 
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role==null)
            {
                ViewBag.ErrorMessage = $"Role with id {roleId} cannot be found.";
                return View("NotFound");
            }
            for (int i = 0; i < model.Count; i++)
            {
              var user = await userManager.FindByIdAsync(model[i].userId);
              IdentityResult result = null;

                if (model[i].isSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].isSelected && (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }

            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }
    }
}
