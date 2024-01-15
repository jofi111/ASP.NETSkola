﻿using ASP.NETSkola.Models;
using ASP.NETSkola.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETSkola.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private UserManager<AppUser> userManager;
        private IPasswordHasher<AppUser> passwordHasher;
        private IPasswordValidator<AppUser> passwordValidator;

        public UsersController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher, IPasswordValidator<AppUser> passwordValidator)
        {
            this.userManager = userManager;
            this.passwordHasher = passwordHasher;
            this.passwordValidator = passwordValidator; 
        }

        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserVM userModel)
        {
            if (ModelState.IsValid)
            {
                AppUser newUser = new AppUser
                {
                    UserName = userModel.Name,
                    Email = userModel.Email,
                };

                var result = await userManager.CreateAsync(newUser, userModel.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(userModel);
        }
        public async Task<IActionResult> Edit(string id)
        {
            AppUser userToEdit = await userManager.FindByIdAsync(id);
            if (userToEdit == null)
            {
                return View("NotFound");
            }
            else
            {
                return View(userToEdit);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            AppUser userToEdit = await userManager.FindByIdAsync(id);
            if (userToEdit == null)
            {
                ModelState.AddModelError("", "User not found");
                return View();
            }
            else
            {
                IdentityResult validPassword = null;
                if (!string.IsNullOrEmpty(email))
                {
                    userToEdit.Email = email;
                }
                else
                {
                    ModelState.AddModelError("", "Fill the e-mail! ;o)");
                }

                if (!string.IsNullOrEmpty(password))
                {
                    //userToEdit.PasswordHash = passwordHasher.HashPassword(userToEdit, password);
                    validPassword = await passwordValidator.ValidateAsync(userManager, userToEdit, password);
                    if (validPassword.Succeeded)
                    {
                        userToEdit.PasswordHash = passwordHasher.HashPassword(userToEdit, password);
                    }
                    else
                    {
                        foreach(var error in validPassword.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else { 
                    ModelState.AddModelError("", "I miss your password <3"); 
                }
                
                //if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                //{
                if (validPassword != null && validPassword.Succeeded)
                    {
                    var saveResult = await userManager.UpdateAsync(userToEdit);
                    if (saveResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in saveResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }  
                }
            }
            return View(userToEdit);
        }

        //Delete - pozor v Index.cshtml je typu btn a nikoliv button typu submit
        public async Task<IActionResult> Delete (string id)
        {    
             AppUser userToDelete = await userManager.FindByIdAsync(id);
             if (userToDelete == null)
             {
                  ModelState.AddModelError("", "User not found :O");
             }
             else
             {
                  var deleteResult = await userManager.DeleteAsync(userToDelete);
                  if (!deleteResult.Succeeded)
                  {
                      ModelState.AddModelError("", "Delete unsuccessfull");
                  }
                    //else
                    //{
                    //    return RedirectToAction("Index");
                    //}
              }
              return RedirectToAction("Index");
         }

    }

}