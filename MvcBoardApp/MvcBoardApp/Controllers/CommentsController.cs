using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcBoardApp.Models;
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
        public IActionResult Create([FromRoute]int boardID, [FromQuery]int pageNumber)
        {
            CreateCommentViewModel createCommentViewModel = new CreateCommentViewModel()
            {
                BoardID = boardID,
                PageIndex = pageNumber
            };

            return View(createCommentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create/{boardID}")]
        public async Task<IActionResult> Create([FromQuery]int pageNumber, [FromForm]CreateCommentViewModel createCommentViewModel)
        {
            TryValidateModel(createCommentViewModel);

            if (ModelState.IsValid)
            {
                Comment comment = new Comment()
                {
                    BoardID = createCommentViewModel.BoardID,
                    CommentUserName = createCommentViewModel.CommentUserName,
                    CommentContent = createCommentViewModel.CommentContent
                };

                mDbContext.Comments.Add(comment);
                await mDbContext.SaveChangesAsync();

                Board board = mDbContext.Boards.FirstOrDefault(m => m.ID == comment.BoardID);
                board.CommentCount = mDbContext.Comments.Count(m => m.BoardID == comment.BoardID);

                mDbContext.SaveChanges();

                return RedirectToAction("Details", "Boards", new { ID = comment.BoardID, pageNumber });
            }

            return View(createCommentViewModel);
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

            EditCommentViewModel editCommentViewModel = new EditCommentViewModel()
            {
                ID = comment.ID,
                BoardID = comment.BoardID,
                CommentUserName = comment.CommentUserName,
                CommentContent = comment.CommentContent,
                PageIndex = pageNumber
            };

            return View(editCommentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{ID}")]
        public async Task<IActionResult> Edit([FromRoute]int? ID, [FromForm]EditCommentViewModel editCommentViewModel, [FromQuery]int pageNumber)
        {
            if (ID != editCommentViewModel.ID)
            {
                return NotFound();
            }

            TryValidateModel(editCommentViewModel);

            if (ModelState.IsValid)
            {
                try
                {

                    Comment comment = await mDbContext.Comments.FirstOrDefaultAsync(m => m.ID == ID);
                    comment.ID = editCommentViewModel.ID;
                    comment.BoardID = editCommentViewModel.BoardID;
                    comment.CommentUserName = editCommentViewModel.CommentUserName;
                    comment.CommentContent = editCommentViewModel.CommentContent;

                    mDbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!commentExists(editCommentViewModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Details", "Boards", new { ID = editCommentViewModel.BoardID, pageNumber = editCommentViewModel.PageIndex });
            }

            return View(editCommentViewModel);
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

            CommentViewModel commentViewModel = new CommentViewModel
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
