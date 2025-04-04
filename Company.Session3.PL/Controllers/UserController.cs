using Company.Session3.DAL.Models;
using Company.Session3.PL.Dtos;
using Company.Session3.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Session3.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<UserToReturnDto> users;
            if (string.IsNullOrEmpty(SearchInput))
            {
                users = _userManager.Users.Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    Email = U.Email,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    PhoneNumber = U.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(U).Result
                });
            }
            else
            {
                users = _userManager.Users.Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    Email = U.Email,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    PhoneNumber = U.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).Where(U => U.FirstName.ToLower().Contains(SearchInput.ToLower()));
            }

            return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound(new { StatusCode = 404, message = $"User With Id  : {id} is not found" });

            var Dto = new UserToReturnDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = _userManager.GetRolesAsync(user).Result
            };

            return View(ViewName, Dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operation!!");

                var user = await _userManager.FindByIdAsync(id);
                if (user is null)
                {
                    return BadRequest("Invalid Operation!!");
                }
                user.UserName = model.UserName;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operation!!");

                var user = await _userManager.FindByIdAsync(id);
                if (user is null)
                {
                    return BadRequest("Invalid Operation!!");
                }

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }
    }
}
