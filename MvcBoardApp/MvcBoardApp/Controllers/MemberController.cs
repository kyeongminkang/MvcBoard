using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcBoardApp.Models;

namespace MvcBoardApp.Controllers
{
    public class MemberController : Controller


    {

        private readonly MvcBoardAppContext _context;

        public MemberController(MvcBoardAppContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
       
         public async Task<IActionResult> Signup([Bind("Id,UserId,UserPassword")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Signup));
            }
            return View(member);
        }

    }
}