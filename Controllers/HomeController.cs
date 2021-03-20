using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using AjaxCsharp.Models;

namespace AjaxCsharp.Controllers
{
    public class HomeController : Controller
    {

        private MyContext _context;

        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult index()
        {
            indexWrapper WMod = new indexWrapper();

            return View("index", WMod);
        }






        // Navigate to other pages--------------------------------- 

        [HttpGet("goToPlanWedding")]
        public IActionResult goToPlanWedding()
        {
            return RedirectToAction("planWedding");
        }

        [HttpGet("goToDashboard")]
        public IActionResult goToDashboard()
        {
            return RedirectToAction("dashboard");
        }


        // -----------------------------------------------------------end

        // Redering pages-----------------------------------------------
        [HttpGet("dashboard")]
        // dashboard
        public IActionResult dashboard()
        {
            // block pages is not in session
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("index");
            }

            // user in session  
            int UserIdInSession = (int)HttpContext.Session.GetInt32("UserId");
            // filter user in session with db
            User UserIndb = _context.Users.FirstOrDefault(u => u.UserId == UserIdInSession);
            ViewBag.ToDisplay = UserIndb;


            // Get all guest from db set


            ViewBag.TodoListItems = _context.TodoLists
           .Where(us => us.UserId == UserIdInSession)
           .ToList();


            DashboardWrapper wMode = new DashboardWrapper();




            return View("dashboard", wMode);
        }


        [HttpGet("displayTodoList")]

        public JsonResult displayTodoList()
        {
            int UserIdInSession = (int)HttpContext.Session.GetInt32("UserId");

            DashboardWrapper wMode = new DashboardWrapper();

            List<TodoList> itemList = _context.TodoLists
            .Where(us => us.UserId == UserIdInSession)
            .ToList();

            //     ViewBag.TodoListItems = _context.TodoLists
            //    .Where(us => us.UserId == UserIdInSession)
            //    .ToList();

            return Json(new { data = itemList });

        }


        [HttpPost("postSale")]
        public IActionResult NewItemHandler(TodoList FromForm)
        {

            // JsonResult
            System.Console.WriteLine("test button was click");
            System.Console.WriteLine("the backend has been reached");
            System.Console.WriteLine($"FromForm: {FromForm}");
            System.Console.WriteLine($"Street name: {FromForm.Item}");

            // get user form session
            int UserIdInSession = (int)HttpContext.Session.GetInt32("UserId");

            // data to save to db
            var Entry = new TodoList
            {
                UserId = UserIdInSession,
                Item = FromForm.Item,

            };

            System.Console.WriteLine($"Entry to be send to db {Entry}");

            // save to db
            _context.Add(Entry);
            _context.SaveChanges();

            return Json(new { Status = "success", FromForm });
        }


        [HttpPost("editItem")]
        public IActionResult EditItemHandler(TodoList editItemId)

        {

            System.Console.WriteLine("you have reach the backend for editing!");
            System.Console.WriteLine($"text from user: {editItemId.Item}");
            System.Console.WriteLine($"need to get id {editItemId.TodoListId}");

            TodoList GetItem = _context.TodoLists.FirstOrDefault(i => i.TodoListId == editItemId.TodoListId);
            GetItem.TodoListId = editItemId.TodoListId;
            GetItem.Item = editItemId.Item;
            _context.SaveChanges();

            return Json(new { Status = "success", editItemId });
        }

        [HttpGet("DeleteHandler")]

        public IActionResult DeleteHandler(TodoList DataId)
        {
            System.Console.WriteLine("you have connected to the backend");
            System.Console.WriteLine($"itemID: {DataId.TodoListId}");

            int UserIdInSession = (int)HttpContext.Session.GetInt32("UserId");


            TodoList GetId = _context.TodoLists.SingleOrDefault(l => l.TodoListId == DataId.TodoListId);

            _context.TodoLists.Remove(GetId);
            _context.SaveChanges();

            return Json(new { Status = "success", DataId });


        }





        // -----------------------------------------------------------end


        // Processing-----------------------------------------








        // Registertion------------------------------------------------
        [HttpPost("Redgister")]
        public IActionResult Redgister(User FromForm)
        {

            // Pass in Models so that parcial can use them
            indexWrapper WMod = new indexWrapper();

            // Check if email is already in db
            if (_context.Users.Any(u => u.Email == FromForm.Email))
            {
                ModelState.AddModelError("Email", "Email already in use!");
            }

            // Validations
            if (ModelState.IsValid)
            {
                // #hash password
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                FromForm.Password = Hasher.HashPassword(FromForm, FromForm.Password);

                // Add to db
                _context.Add(FromForm);
                _context.SaveChanges();

                // Session
                HttpContext.Session.SetInt32("UserId", _context.Users.FirstOrDefault(i => i.UserId == FromForm.UserId).UserId);
                // Redirect
                System.Console.WriteLine("You may contine!");
                return RedirectToAction("dashboard", WMod);
            }
            else
            {
                System.Console.WriteLine("Fix your erros!");
                return View("index", WMod);

            }

        }



        // Login-------------------------------------------------    
        [HttpPost("Login")]
        public IActionResult Login(LoginUser userSubmission)
        {

            indexWrapper WMod = new indexWrapper();

            // Validations
            if (ModelState.IsValid)
            {

                // Check db email with from email
                var userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.LogEmail);

                // No user in db
                if (userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("index", WMod);
                }
                // Check hashing are the same
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LogPassword);

                if (result == 0)
                {
                    // ModelState.AddModelError("Email", "Invalid Email/Password");
                    // return View("index", WMod);// handle failure (this should be similar to how "existing email" is handled)
                }
                // Set Session Instance
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);

                return RedirectToAction("dashboard", WMod);

            }

            return View("index", WMod);

        }


        [HttpGet("logout")]
        public IActionResult logout()
        {
            // Clear Session
            HttpContext.Session.Clear();
            return RedirectToAction("index");
        }


        // ------------------------------------------end of regitration and login







    }

}