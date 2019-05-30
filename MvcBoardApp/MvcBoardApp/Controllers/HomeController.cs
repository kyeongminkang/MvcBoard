using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcBoardApp.Models;

namespace MvcBoardApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


     



        [Route("Home/privacy")]
        public IActionResult Privacy()
        {



            Board board = new Board()
            {
                Content = "모르겠다"
            };

            //BoardCommentModel boardCommentModel = new BoardCommentModel()
            //{
            //    memo = "abcdefghtifjkasdl"
            //};

            BoardDetailsViewModel bvm = new BoardDetailsViewModel
            {
                memo = "모르겠다"
            };


           
            return View(bvm);










            //return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
