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

            var comment = await _context.Comment.FirstOrDefaultAsync(m => m.ID == id);

            if (comment == null)
            {
                return NotFound();
            }

            BoardComment BC = new BoardComment();
            BC.Comment = comment;
            BC.PageIndex = (int)pageNumber;

            return View(BC);
        }

        [HttpGet]
        [Route("Create/{boardid}")]
        public IActionResult Create(int? id, Comment comment, int? pageNumber)
        {
            BoardComment BC = new BoardComment();
            BC.Comment = comment;
            BC.PageIndex = (int)pageNumber;
            return View(BC);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create/{id}")]
        public async Task<IActionResult> Create(Comment comment, int? pageNumber)
        {
            if (ModelState.IsValid)
            {
                _context.Comment.Add(comment);
                await _context.SaveChangesAsync();

                Board board = _context.Board.FirstOrDefault(m => m.ID == comment.BoardID);

                var commentCounter = new CommentCounter
                {
                    ID = (int)comment.BoardID,
                    CommentCount = _context.Comment.Count(m => m.BoardID == comment.BoardID)
                };

                commentCounter.GetCount(board);
                _context.SaveChanges();

                BoardComment boardComment = new BoardComment
                {
                    PageIndex = (int)pageNumber
                };

                return RedirectToAction("Details", "Boards", new { id = comment.BoardID, pageNumber = boardComment.PageIndex });
            }
            return View(comment);
        }

        public int Counter(int? id)
        {
            return _context.Comment.Count(m => m.BoardID == id);
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

            BoardComment boardComment = new BoardComment();
            boardComment.Comment = comment;
            boardComment.PageIndex = (int)pageNumber;
            return View(boardComment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int? id, [Bind("ID,CommentUserName,CommentContent,BoardID")] Comment comment, int? pageNumber)
        {
            if (id != comment.ID)
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
                    if (!CommentExists((int)comment.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                BoardComment boardComment = new BoardComment();
                boardComment.PageIndex = (int)pageNumber;

                return RedirectToAction("Details", "Boards", new { id = comment.BoardID, pageNumber = boardComment.PageIndex });
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
                .FirstOrDefaultAsync(m => m.ID == id);
            if (comment == null)
            {
                return NotFound();
            }

            BoardComment boardComment = new BoardComment();
            boardComment.Comment = comment;
            boardComment.PageIndex = (int)pageNumber;

            return View(boardComment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, int? pageNumber)
        {
            var comment = await _context.Comment.FindAsync(id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();

            Board b = _context.Board.FirstOrDefault(m => m.ID == comment.BoardID);

            var cc = new CommentCounter
            {
                ID = (int)comment.BoardID,
                CommentCount = _context.Comment.Count(m => m.BoardID == comment.BoardID)
            };

            cc.GetCount(b);
            _context.SaveChanges();

            BoardComment boardComment = new BoardComment();
            boardComment.PageIndex = (int)pageNumber;

            return RedirectToAction("Details", "Boards", new { id = comment.BoardID, pageNumber = boardComment.PageIndex });
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.ID == id);
        }
    }
}
