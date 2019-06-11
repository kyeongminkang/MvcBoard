using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcBoardApp.Models;
using MvcBoardApp.ViewModels;

namespace MvcBoardApp.Controllers
{

    [Route("Boards")]
    public class BoardsController : Controller
    {
        private readonly MvcBoardAppContext _context;

        public BoardsController(MvcBoardAppContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> Index(string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            ViewData["CuurentSort"] = sortOrder;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var board = from m in _context.Board
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                board = board.Where(s => s.Subject.Contains(searchString));
            }

            board = board.OrderByDescending(s => s.ID);

            int pageSize = 5;

            return View(await PaginatedList<Board>.CreateAsync(board.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        [Route("Details/{id}")]
        public async Task<IActionResult> Details(int? id, int? pageNumber)
        {

            if (id == null)
            {
                return NotFound();
            }

            var board = await _context.Board.FirstOrDefaultAsync(m => m.ID == id);

            if (board == null)
            {
                return NotFound();
            }

            BoardComment boardComment = new BoardComment
            {
                Board = board,
                Comments = GetComment(id),
                PageIndex = (int)pageNumber
            };

            return View(boardComment);
        }

        public List<Comment> GetComment(int? id)
        {
            return _context.Comment.Where(m => m.BoardID == id).ToList();
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> Create(int? pageNumber)
        {
            var board = from m in _context.Board
                        select m;

            int pageSize = 5;

            return View(await PaginatedList<Board>.CreateAsync(board.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create([Bind("ID,UserName,Subject,Content,WriteDate,CommentCount")] Board board, int? pageNumber)
        {
            if (ModelState.IsValid)
            {
                _context.Add(board);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Boards", new { pageNumber = pageNumber });
            }

            return View(board);
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id, int? pageNumber)
        {
            if (id == null)
            {
                return NotFound();
            }

            var board = await _context.Board.FindAsync(id);

            if (board == null)
            {
                return NotFound();
            }

            BoardComment boardComment = new BoardComment
            {
                Board = board,
                PageIndex = (int)pageNumber
            };

            return View(boardComment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id, [Bind("ID,UserName,Subject,Content,WriteDate,CommentCount")] Board board, int pageNumber)
        {
            if (id != board.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(board);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoardExists((int)board.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Boards", new { pageNumber = pageNumber });
            }
            return View(board);
        }

        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int? id, int? pageNumber)
        {
            if (id == null)
            {
                return NotFound();
            }

            var board = await _context.Board.FirstOrDefaultAsync(m => m.ID == id);

            if (board == null)
            {
                return NotFound();
            }

            BoardComment boardComment = new BoardComment
            {
                Board = board,
                PageIndex = (int)pageNumber
            };

            return View(boardComment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id, int? pageNumber)
        {
            var board = await _context.Board.FindAsync(id);
            _context.Board.Remove(board);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Boards", new { pageNumber = pageNumber });
        }

        private bool BoardExists(int id)
        {
            return _context.Board.Any(e => e.ID == id);
        }
    }
}
