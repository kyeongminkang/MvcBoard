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
        [Route("Create")]
        public IActionResult Create([FromQuery]int boardID, Comment comment, [FromQuery]int pageNumber)
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
        [Route("Create")]
        public async Task<IActionResult> Create(Comment comment, [FromQuery]int pageNumber)
        {
            if (ModelState.IsValid)
            {
                mDbContext.Comments.Add(comment);
                await mDbContext.SaveChangesAsync();

                Board board = mDbContext.Boards.FirstOrDefault(m => m.ID == comment.BoardID);

                var boardViewModel = new BoardViewModel()
                {
                    CommentCount = mDbContext.Comments.Count(m => m.BoardID == comment.BoardID )
                };
                boardViewModel.GetCount(board);

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
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromQuery]int? ID, [FromQuery]int pageNumber)
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
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromQuery]int? ID, Comment comment, [FromQuery]int pageNumber)
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
                    if (!CommentExists(comment.ID))
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
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromQuery]int? ID, [FromQuery]int pageNumber)
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete")]
        public async Task<IActionResult> DeleteConfirmed([FromQuery]int ID, [FromQuery]int pageNumber)
        {
            var comment = await mDbContext.Comments.FindAsync(ID);
            mDbContext.Comments.Remove(comment);
            await mDbContext.SaveChangesAsync();

            Board board = mDbContext.Boards.FirstOrDefault(m => m.ID == comment.BoardID);

            var boardViewModel = new BoardViewModel()
            {
                CommentCount = mDbContext.Comments.Count(m => m.BoardID == comment.BoardID)
            };
            boardViewModel.GetCount(board);

            mDbContext.SaveChanges();

            CommentViewModel commentViewModel = new CommentViewModel
            {
                PageIndex = pageNumber
            };

            return RedirectToAction("Details", "Boards", new { ID = comment.BoardID, pageNumber = commentViewModel.PageIndex });
        }

        private bool CommentExists(int ID)
        {
            return mDbContext.Comments.Any(e => e.ID == ID);
        }
    }
}
