using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcBoardApp.Models;
using MvcBoardApp.ViewModels;
using MvcBoardApp.Models.ViewModels;
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
        [Route("Create/{boardID}")]
        public IActionResult Create([FromRoute]int boardID, Comment comment, [FromQuery]int pageNumber)
        {

            CreateCommentViewModel createCommentViewModel = new CreateCommentViewModel()
            {
                ID = comment.ID,
                BoardID = comment.BoardID,


                PageIndex = pageNumber
            };

            return View(createCommentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create/{boardID}")]
        public async Task<IActionResult> Create(Comment comment, [FromQuery]int pageNumber, [FromForm]CreateCommentViewModel createCommentViewModel)
        {
            if (ModelState.IsValid)
            {
                mDbContext.Comments.Add(comment);
                await mDbContext.SaveChangesAsync();

                Board board = mDbContext.Boards.FirstOrDefault(m => m.ID == comment.BoardID);
                board.CommentCount = mDbContext.Comments.Count(m => m.BoardID == comment.BoardID);

                mDbContext.SaveChanges();

                var commentViewModel = new CommentViewModel
                {
                    PageIndex = pageNumber
                };

                return RedirectToAction("Details", "Boards", new { ID = comment.BoardID, pageNumber = commentViewModel.PageIndex });
            }

            return View(comment);
        }

        [HttpGet]
        [Route("Edit/{ID}")]
        public async Task<IActionResult> Edit([FromRoute]int? ID, [FromQuery]int pageNumber)
        {
            if (ID == null)
            {
                return NotFound();
            }

            Comment comment = await mDbContext.Comments.FindAsync(ID);

            if (comment == null)
            {
                return NotFound();
            }

            var commentViewModel = new CommentViewModel
            {
                Comment = comment,
                PageIndex = pageNumber
            };

            return View(commentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{ID}")]
        public async Task<IActionResult> Edit([FromRoute]int? ID, Comment comment, [FromQuery]int pageNumber)
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

                var commentViewModel = new CommentViewModel
                {
                    PageIndex = pageNumber
                };

                return RedirectToAction("Details", "Boards", new { ID = comment.BoardID, pageNumber = commentViewModel.PageIndex });
            }

            return View(comment);
        }

        [HttpGet]
        [Route("Delete/{ID}")]
        public async Task<IActionResult> Delete([FromRoute]int? ID, [FromQuery]int pageNumber)
        {
            if (ID == null)
            {
                return NotFound();
            }

            Comment comment = await mDbContext.Comments.FirstOrDefaultAsync(m => m.ID == ID);

            if (comment == null)
            {
                return NotFound();
            }

            var commentViewModel = new CommentViewModel
            {
                Comment = comment,
                PageIndex = pageNumber
            };

            return View(commentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete/{ID}")]
        public async Task<IActionResult> Delete([FromRoute]int ID, [FromQuery]int pageNumber)
        {
            Comment comment = await mDbContext.Comments.FindAsync(ID);
            mDbContext.Comments.Remove(comment);
            await mDbContext.SaveChangesAsync();

            Board board = mDbContext.Boards.FirstOrDefault(m => m.ID == comment.BoardID);
            board.CommentCount = mDbContext.Comments.Count(m => m.BoardID == comment.BoardID);

            mDbContext.SaveChanges();

            var commentViewModel = new CommentViewModel
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
