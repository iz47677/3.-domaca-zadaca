using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Interfaces;
using EntityFramework;

namespace AspNet.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoController(ITodoRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            return View(_repository.GetActive(new Guid(currentUser.Id)));
        }

        [Authorize]
        public IActionResult Add()
        {
            ViewData["Error"] = "false";
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Completed()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            return View(_repository.GetCompleted(new Guid(currentUser.Id)));
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel model)
        {
            if (model.Text == null)
            {
                ViewData["Error"] = "true";
                return View();
            }
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.Add(new TodoItem(model.Text, new Guid(currentUser.Id)));
            return View("Index", _repository.GetActive(new Guid(currentUser.Id)));
        }

        [Authorize]
        public async Task<IActionResult> MarkAsCompleted(Guid todoId)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.MarkAsCompleted(todoId, new Guid(currentUser.Id));
            return View("Index", _repository.GetActive(new Guid(currentUser.Id)));
        }
    }
}