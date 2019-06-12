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
        private readonly MvcBoardAppContext mDbContext;

        public CommentsController(MvcBoardAppContext context)
        {
            mDbContext = context;
        }

        [HttpGet]
        [Route("Create/{boardid}")]
        public IActionResult Create(int? id, Comment comment, int? pageNumber)
        {
           
            CommentViewModel commentViewModel = new CommentViewModel
            {
                Comment = comment,
                PageIndex = (int)pageNumber
            };

            return View(commentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create/{id}")]
        public async Task<IActionResult> Create(Comment comment, int? pageNumber)
        {
            if (ModelState.IsValid)
            {
                mDbContext.Comment.Add(comment);
                await mDbContext.SaveChangesAsync();

                Board board = mDbContext.Board.FirstOrDefault(m => m.ID == comment.BoardID);

                var commentCounter = new CommentCounter
                {
                    ID = (int)comment.BoardID,
                    CommentCount = mDbContext.Comment.Count(m => m.BoardID == comment.BoardID)
                };

                commentCounter.GetCount(board);
                mDbContext.SaveChanges();

                CommentViewModel commentViewModel = new CommentViewModel
                {
                    PageIndex = (int)pageNumber
                };

                return RedirectToAction("Details", "Boards", new { id = comment.BoardID, pageNumber = commentViewModel.PageIndex });
            }

            return View(comment);
        }

        public int Counter(int? id)
        {
            return mDbContext.Comment.Count(m => m.BoardID == id);
        }

        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int? id, int? pageNumber)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await mDbContext.Comment.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            CommentViewModel commentViewModel = new CommentViewModel
            {
                Comment = comment,
                PageIndex = (int)pageNumber
            };

            return View(commentViewModel);
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
                    mDbContext.Update(comment);
                    await mDbContext.SaveChangesAsync();
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

                CommentViewModel commentViewModel = new CommentViewModel
                {
                    PageIndex = (int)pageNumber
                };

                return RedirectToAction("Details", "Boards", new { id = comment.BoardID, pageNumber = commentViewModel.PageIndex });
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

            var comment = await mDbContext.Comment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (comment == null)
            {
                return NotFound();
            }

            CommentViewModel commentViewModel = new CommentViewModel
            {
                Comment = comment,
                PageIndex = (int)pageNumber
            };

            return View(commentViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, int? pageNumber)
        {
            var comment = await mDbContext.Comment.FindAsync(id);
            mDbContext.Comment.Remove(comment);
            await mDbContext.SaveChangesAsync();

            Board b = mDbContext.Board.FirstOrDefault(m => m.ID == comment.BoardID);

            var commentCounter = new CommentCounter
            {
                ID = (int)comment.BoardID,
                CommentCount = mDbContext.Comment.Count(m => m.BoardID == comment.BoardID)
            };

            commentCounter.GetCount(b);
            mDbContext.SaveChanges();

            CommentViewModel commentViewModel = new CommentViewModel
            {
                PageIndex = (int)pageNumber
            };

            return RedirectToAction("Details", "Boards", new { id = comment.BoardID, pageNumber = commentViewModel.PageIndex });
        }

        private bool CommentExists(int id)
        {
            return mDbContext.Comment.Any(e => e.ID == id);
        }
    }
}
