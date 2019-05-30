﻿using System;
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
        // GET: Boards
        public async Task<IActionResult> Index(string searchString)
        {
            
            //return View(await _context.Board.ToListAsync());
            var boards = from m in _context.Board
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                boards = boards.Where(s => s.Subject.Contains(searchString));
            }
            return View(await boards.ToListAsync());
            
        }




        [HttpGet]
        // GET: Boards/Details/5
        [Route("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
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




            return View(board);
        }
     
     

        [HttpGet]
        [Route("Create")]
        // GET: Boards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Boards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create([Bind("Id,UserName,Subject,Content,WriteTime,Hits")] Board board)
        {
            if (ModelState.IsValid)
            {
                _context.Add(board);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(board);
        }

        [HttpGet]
        [Route("Edit")]
        // GET: Boards/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            return View(board);
        }

        // POST: Boards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Subject,Content,WriteTime,Hits")] Board board)
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
                return RedirectToAction(nameof(Index));
            }
            return View(board);
        }

        [HttpGet]
        [Route("Delete")]
        // GET: Boards/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(board);
        }

        // POST: Boards/Delete/5
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
