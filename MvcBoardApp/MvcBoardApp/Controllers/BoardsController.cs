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


        int? pageIndex;

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

            var boards = from m in _context.Board
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                boards = boards.Where(s => s.Subject.Contains(searchString));
            }

            int pageSize = 5;
            pageIndex = pageNumber;
            

            return View(await PaginatedList<Board>.CreateAsync(boards.AsNoTracking(), pageNumber ?? 1, pageSize));

        }
        
        [HttpGet]
        [Route("Details/{id}")]
        public async Task<IActionResult> Details(int? id, int? pageNumber)
        {

            if (id == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .FirstOrDefaultAsync(m => m.Id == id);

            if (board == null)
            {
                return NotFound();
            }

            BoardComment bC = new BoardComment();
            
            bC.Board = board;
            bC.Comments = GetComment(id);
              
            return View(bC);
        }

        public List<Comment> GetComment(int? id)
        {
            return _context.Comment.Where(m => m.BoardId == id).ToList();
        
        }

        [HttpGet]
        [Route("Create")]
        //public IActionResult Create(int? PageNumber)
        //{


        //    return View(PageNumber);
        //}


        public async Task<IActionResult> Create(int? pageNumber)
        {

            var boards = from m in _context.Board
                         select m;

            
            int pageSize = 5;
            
            return View(await PaginatedList<Board>.CreateAsync(boards.AsNoTracking(), pageNumber ?? 1, pageSize));

        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create([Bind("Id,UserName,Subject,Content,WriteDate,CommentCount")] Board board, int? PageNumber)
        {
            if (ModelState.IsValid)
            {
                _context.Add(board);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                
                return RedirectToAction("Index", "Boards", new { pageNumber = PageNumber });
            }
            return View(board);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public async Task<IActionResult> Edit(int? id, int? pageNumber, string[] paths=null)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var board = await _context.Board.FindAsync(id);
            //var board = from m in _context.Board where m.Id==id select m;
            //var board = _context.Board.Where(m => m.Id == id);
    
            //foreach(var path in paths)
            //{
            //    _context.Entry(board).Reference(path).Load();
            //}

            if (board == null)
            {
                return NotFound();
            }
            //int pageSize = 5;
            

            //return View(await PaginatedList<Board>.CreateAsync(board.AsNoTracking(), pageNumber ?? 1, pageSize));
            return View(board);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id?}")]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,UserName,Subject,Content,WriteDate,CommentCount")] Board board, int PageNumber)
        {
            if (id != board.Id)
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
                    if (!BoardExists(board.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Boards", new { pageNumber = PageNumber });
            }
            return View(board);
        }

        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var board = await _context.Board.FirstOrDefaultAsync(m => m.Id == id);

            if (board == null)
            {
                return NotFound();
            }
            return View(board);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var board = await _context.Board.FindAsync(id);
            _context.Board.Remove(board);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoardExists(int id)
        {
            return _context.Board.Any(e => e.Id == id);
        }
    }
}
