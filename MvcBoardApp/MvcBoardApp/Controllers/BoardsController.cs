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
using MvcBoardApp.Models.ViewModels;

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
        public async Task<IActionResult> Index([FromQuery]string searchString, [FromQuery]string sortOrder, [FromQuery]string currentFilter, [FromQuery]int? pageNumber)
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

            var boards = mDbContext.Boards.AsQueryable().AsNoTracking();

            if (!string.IsNullOrEmpty(searchString))
            {
                boards = boards.Where(s => s.Subject.Contains(searchString));
            }

            boards = boards.OrderByDescending(s => s.ID);

            int pageSize = 5;

            return View(await PaginatedList<Board>.CreateAsync(boards.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        [Route("Details/{ID}")]
        public async Task<IActionResult> Details([FromRoute]int? ID, [FromQuery]int pageNumber)
        {

            if (ID == null)
            {
                return NotFound();
            }

            Board board = await mDbContext.Boards.FirstOrDefaultAsync(m => m.ID == ID);

            if (board == null)
            {
                return NotFound();
            }

            var boardViewModel = new BoardViewModel
            {
                Board = board, 
                Comments = await mDbContext.Comments.Where(m => m.BoardID == ID).ToListAsync(),
                PageIndex = pageNumber
            };

            return View(boardViewModel);
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create([FromQuery]int pageNumber)
        {
            var boardViewModel = new BoardViewModel()
            {
                PageIndex = pageNumber
            };

            return View(boardViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create([FromForm]Board board, [FromQuery]int pageNumber)
        {
            
            CreateBoardViewModel createBoardViewModel = new CreateBoardViewModel();

            

            if (ModelState.IsValid)
            {
                mDbContext.Add(board);
                await mDbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Boards", new { pageNumber });
            }

            return View(board);
        }

        [HttpGet]
        [Route("Edit/{ID}/{pageNumber}")]
        public async Task<IActionResult> Edit([FromRoute]int? ID, [FromRoute]int pageNumber)
        {
            if (ID == null)
            {
                return NotFound();
            }

            Board board = await mDbContext.Boards.FindAsync(ID);

            if (board == null)
            {
                return NotFound();
            }

            var boardViewModel = new BoardViewModel
            {
                Board = board,
                PageIndex = pageNumber
            };

            return View(boardViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{ID}/{pageNumber}")]
        public async Task<IActionResult> Edit([FromRoute]int ID, Board board, [FromRoute]int pageNumber)
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
                    if (!boardExists(board.ID))
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
        [Route("Delete/{ID}/{pageNumber}")]
        public async Task<IActionResult> Delete([FromRoute]int? ID, [FromRoute]int pageNumber)
        {
            if (ID == null)
            {
                return NotFound();
            }

            Board board = await mDbContext.Boards.FirstOrDefaultAsync(m => m.ID == ID);

            if (board == null)
            {
                return NotFound();
            }

            var boardViewModel = new BoardViewModel
            {
                Board = board,
                PageIndex = pageNumber
            };

            return View(boardViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete/{ID}/{pageNumber}")]
        public async Task<IActionResult> Delete([FromRoute]int ID, [FromRoute]int pageNumber)
        {
            Board board = await mDbContext.Boards.FindAsync(ID);
            mDbContext.Boards.Remove(board);
            await mDbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Boards", new { pageNumber });
        }

        private bool boardExists(int ID)
        {
            return mDbContext.Boards.Any(e => e.ID == ID);
        }
    }
}
