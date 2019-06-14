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
        [Route("Create/{boardID}/{pageNumber}")]
        public IActionResult Create(int boardID, Comment comment, int pageNumber)
        {

            CommentViewModel commentViewModel = new CommentViewModel
            {
                Comment = comment,
                PageIndex = pageNumber
            };

            return View(commentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create/{ID}/{pageNumber}")]
        public async Task<IActionResult> Create(Comment comment, int pageNumber)
        {
            if (ModelState.IsValid)
            {
                mDbContext.Comments.Add(comment);
                await mDbContext.SaveChangesAsync();

                Board board = mDbContext.Boards.FirstOrDefault(m => m.ID == comment.BoardID);
                board.CommentCount = mDbContext.Comments.Count(m => m.BoardID == comment.BoardID);

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
            return mDbContext.Comments.Count(m => m.BoardID == ID);
        }

        [HttpGet]
        [Route("Edit/{ID}/{pageNumber}")]
        public async Task<IActionResult> Edit(int? ID, int pageNumber)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var comment = await mDbContext.Comments.FindAsync(ID);

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
        [Route("Edit/{ID}/{pageNumber}")]
        public async Task<IActionResult> Edit(int? ID, Comment comment, int pageNumber)
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
                    if (!commentExists(comment.ID))
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
                    PageIndex = pageNumber
                };

                return RedirectToAction("Details", "Boards", new { ID = comment.BoardID, pageNumber = commentViewModel.PageIndex });
            }

            return View(comment);
        }

        [HttpGet]
        [Route("Delete/{ID}/{pageNumber}")]
        public async Task<IActionResult> Delete(int? ID, int pageNumber)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var comment = await mDbContext.Comments
                .FirstOrDefaultAsync(m => m.ID == ID);
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
        [Route("Delete/{ID}/{pageNumber}")]
        public async Task<IActionResult> Delete(int ID, int pageNumber)
        {
            var comment = await mDbContext.Comments.FindAsync(ID);
            mDbContext.Comments.Remove(comment);
            await mDbContext.SaveChangesAsync();

            Board board = mDbContext.Boards.FirstOrDefault(m => m.ID == comment.BoardID);
            board.CommentCount = mDbContext.Comments.Count(m => m.BoardID == comment.BoardID);

            mDbContext.SaveChanges();

            CommentViewModel commentViewModel = new CommentViewModel
            {
                PageIndex = pageNumber
            };

            return RedirectToAction("Details", "Boards", new { ID = comment.BoardID, pageNumber = commentViewModel.PageIndex });
        }

        private bool commentExists(int ID)
        {
            return mDbContext.Comments.Any(e => e.ID == ID);
        }
    }
}
