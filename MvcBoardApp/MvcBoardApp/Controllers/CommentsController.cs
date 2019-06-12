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
        [Route("Create/{BoardID}")]
        public IActionResult Create(int? boardID, Comment comment, int pageNumber)
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
        [Route("Create/{BoardID}")]
        public async Task<IActionResult> Create(Comment comment, int pageNumber)
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
                    PageIndex = pageNumber
                };

                return RedirectToAction("Details", "Boards", new { ID = comment.BoardID, pageNumber = commentViewModel.PageIndex });
            }

            return View(comment);
        }

        public int Counter(int? ID)
        {
            return mDbContext.Comment.Count(m => m.BoardID == ID);
        }

        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int? ID, int pageNumber)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var comment = await mDbContext.Comment.FindAsync(ID);

            if (comment == null)
            {
                return NotFound();
            }

            CommentViewModel commentViewModel = new CommentViewModel
            {
                Comment = comment,
                PageIndex = pageNumber
            };

            return View(commentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int? ID, Comment comment, int? pageNumber)
        {
            if (ID != comment.ID)
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

                return RedirectToAction("Details", "Boards", new { ID = comment.BoardID, pageNumber = commentViewModel.PageIndex });
            }

            return View(comment);
        }

        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int? ID, int pageNumber)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var comment = await mDbContext.Comment
                .FirstOrDefaultAsync(m => m.ID == ID);
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
        public async Task<IActionResult> DeleteConfirmed([FromQuery]int ID, int pageNumber)
        {
            var comment = await mDbContext.Comment.FindAsync(ID);
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

            return RedirectToAction("Details", "Boards", new { ID = comment.BoardID, pageNumber = commentViewModel.PageIndex });
        }

        private bool CommentExists(int ID)
        {
            return mDbContext.Comment.Any(e => e.ID == ID);
        }
    }
}
