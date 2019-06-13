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
        private readonly MvcBoardAppContext mDbContext;

        public BoardsController(MvcBoardAppContext context)
        {
            mDbContext = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchstring, string sortOrder, string currentFilter, int? pageNumber)
        {
            ViewData["CuurentSort"] = sortOrder;

            if (searchstring != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchstring = currentFilter;
            }

            ViewData["CurrentFilter"] = searchstring;

<<<<<<< HEAD
            var board = from m in _context.Board.AsQueryable() select m;
=======
            var boards = mDbContext.Board.AsQueryable();
>>>>>>> feature/연습용

            if (!String.IsNullOrEmpty(searchstring))
            {
                boards = boards.Where(s => s.Subject.Contains(searchstring));
            }

            boards = boards.OrderByDescending(s => s.ID);

            int pageSize = 5;

            return View(await PaginatedList<Board>.CreateAsync(boards.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        [Route("Details")]
        public async Task<IActionResult> Details([FromQuery]int? ID, [FromQuery]int pageNumber)
        {

            if (ID == null)
            {
                return NotFound();
            }

            var board = await mDbContext.Board.FirstOrDefaultAsync(m => m.ID == ID);

            if (board == null)
            {
                return NotFound();
            }

            BoardViewModel boardViewModel = new BoardViewModel
            {
                Board = board,
                Comments = mDbContext.Comment.Where(m => m.BoardID == ID).ToList(),
                PageIndex = pageNumber
            };

            return View(boardViewModel);
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create([FromQuery]int pageNumber)
        {
            BoardViewModel boardViewModel = new BoardViewModel()
            {
                PageIndex = pageNumber
            };

            return View(boardViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create(Board board, [FromQuery]int pageNumber)
        {
            if (ModelState.IsValid)
            {
                mDbContext.Add(board);
                await mDbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Boards", new { pageNumber });
            }

            return View(board);
        }

        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromQuery]int? ID, [FromQuery]int pageNumber)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var board = await mDbContext.Board.FindAsync(ID);

            if (board == null)
            {
                return NotFound();
            }

            BoardViewModel boardViewModel = new BoardViewModel
            {
                Board = board,
                PageIndex = pageNumber
            };

            return View(boardViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromQuery]int? ID, Board board, [FromQuery]int pageNumber)
        {
            if (ID != board.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    mDbContext.Update(board);
                    await mDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoardExists(board.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index", "Boards", new { pageNumber });
            }

            return View(board);
        }

        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromQuery]int? ID, [FromQuery]int pageNumber)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var board = await mDbContext.Board.FirstOrDefaultAsync(m => m.ID == ID);

            if (board == null)
            {
                return NotFound();
            }

            BoardViewModel boardViewModel = new BoardViewModel
            {
                Board = board,
                PageIndex = pageNumber
            };

            return View(boardViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? ID, [FromQuery]int pageNumber)
        {
            var board = await mDbContext.Board.FindAsync(ID);
            mDbContext.Board.Remove(board);
            await mDbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Boards", new { pageNumber });
        }

        private bool BoardExists(int ID)
        {
            return mDbContext.Board.Any(e => e.ID == ID);
        }
    }
}
