using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcBoardApp.Models;
using MvcBoardApp.ViewModels;
using MvcBoardApp.Controllers;

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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Comment.ToListAsync());
        }

        [HttpGet]
        [Route("Details")]
        public async Task<IActionResult> Details(int? id, int? pageNumber)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FirstOrDefaultAsync(m => m.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            Page p = new Page();
            p.comment = comment;
            p.PageIndex = (int)pageNumber;

            return View(p);
        }

        [HttpGet]
        [Route("Create/{boardid}")]
        public IActionResult Create(int? id, Comment comment, int? pageNumber)
        {
            Page p = new Page();
            p.comment = comment;
            p.PageIndex = (int)pageNumber;
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create/{id?}")]
        public async Task<IActionResult> Create(Comment comment, int? pageNumber)
        {
            if (ModelState.IsValid)
            {

                _context.Comment.Add(comment);
                await _context.SaveChangesAsync();

                Board b = _context.Board.FirstOrDefault(m => m.Id == comment.BoardId);

                var cc = new CommentCounter
                {
                    Id = comment.BoardId,
                    CommentCount = _context.Comment.Count(m => m.BoardId == comment.BoardId)
                };

                cc.MapToModel(b);
                _context.SaveChanges();
                
                Page p = new Page();
                p.PageIndex = (int)pageNumber;

                return RedirectToAction("Details", "Boards", new { id = comment.BoardId, pageNumber = p.PageIndex });
            }
            return View(comment);
        }

        public int Counter(int? id)
        {
            return _context.Comment.Count(m => m.BoardId == id);
        }

        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int? id, int? pageNumber)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }
        
            Page p = new Page();
            p.comment = comment;
            p.PageIndex = (int)pageNumber;
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,CommentUserName,CommentContent,BoardId")] Comment comment, int? pageNumber)
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

                Page p = new Page();
                p.PageIndex = (int)pageNumber;
                return RedirectToAction("Details", "Boards", new { id = comment.BoardId, pageNumber = p.PageIndex });
            }
            return View(comment);
        }

        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int? id, int? pageNumber)
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

            Page p = new Page();
            p.comment = comment;
            p.PageIndex = (int)pageNumber;
            return View(p);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, int? pageNumber)
        {
            var comment = await _context.Comment.FindAsync(id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();

            Board b = _context.Board.FirstOrDefault(m => m.Id == comment.BoardId);

            var cc = new CommentCounter
            {
                Id = comment.BoardId,
                CommentCount = _context.Comment.Count(m => m.BoardId == comment.BoardId)
            };

            cc.MapToModel(b);
            _context.SaveChanges();

            Page p = new Page();
            p.PageIndex = (int)pageNumber;

            return RedirectToAction("Details", "Boards", new { id = comment.BoardId, pageNumber = p.PageIndex });
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }
    }
}
