using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcBoardApp.Models;

namespace MvcBoardApp.Controllers
{

    [Route("Comments")]
    public class CommentsController : Controller
    {
        private readonly MvcBoardAppContext _context;

        public CommentsController(MvcBoardAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        // GET: Comments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Comment.ToListAsync());
        }

        [HttpGet]
        [Route("Details")]
        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [HttpGet]
        // GET: Comments/Create
        [Route("Create/{boardid}")]
        public IActionResult Create(int boardid, Comment comment)
        {
          

            return View(comment);
        }

        //public IActionResult Create(int id, Comment comment)
        //{
        //    return View(comment);
        //}

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create/{id?}")]
        public async Task<IActionResult> Create (int? boardid, int? id, Comment comment, Board board)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return View("~/Views/Boards/details.cshtml");
                //return RedirectToAction(nameof(Index));
            }



            return View(comment);
        }


        //public async Task<IActionResult> Create([Bind("Id,UserName,content,boardid")] Comment comment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(comment);
        //        await _context.SaveChangesAsync();
        //       
        //    }
        //    return View(comment);
        //}

        [HttpGet]
        [Route("Edit")]
        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id, Comment comment)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment1 = await _context.Comment.FindAsync(id);
            if (comment1 == null)
            {
                return NotFound();
            }
            return View(comment1);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,content,boardid")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    comment.BoardId = comment.BoardId;
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
        }

        [HttpGet]
        [Route("Delete")]
        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.FindAsync(id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }
    }
}
