using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcBoardApp.Models;



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
        //public async Task<IActionResult> Details(int? id)
        //{


        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var board = await _context.Board
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (board == null)
        //    {
        //        return NotFound();
        //    }




        //    return View(board);
        //}

        //public ActionResult Details(int? id)
        //{
        //    Comment cm = new Comment();
        //    List<BoardDetailsViewModel> Bdlist = new List<BoardDetailsViewModel>();
        //    var list = (from b in cm.i)
        //}
        


        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var list = await _context.Board.FirstOrDefaultAsync(m => m.Id == id);


            var list2 = await _context.Comment.FirstOrDefaultAsync(m => m.BoardId == id);

            List<Comment> commVMList = new List<Comment>();

            //var commList = (from t in _context.Comment join 
            //                com in _context.Board on t.BoardId equals com.Id
            //                selectnew { t.content,com.}).ToList();

            //foreach (var item in commList)
            //{
            //    BoardDetailsViewModel bdvm = new BoardDetailsViewModel();

            //    bdvm.Id = item.

            //}


            //IEnumerable<String> c_comments;

            //var commList = (from t in _context.Comment where t.BoardId == id select t).ToList;


            //commList = await _context.Comment.ToArrayAsync();

            //c_comments = await _context.Comment.ForEachAsync();
             //c_comments = from t in _context.Comment where t.BoardId == id
             //              select t.content; 


            //BoardDetailsViewModel bd = new BoardDetailsViewModel
            //{
            //    UserName = list.UserName,
            //    Subject = list.Subject,
            //    Content = list.Content,

               // C_Content = list2.content,
              // CommentObject = c_comments.GetEnumerator
                
           // };


            //var list = await _context.Board.FirstOrDefaultAsync(m => m.Id == id);

            // //List<BoardDetailsViewModel> bd = new List<BoardDetailsViewModel>();

            // List<Board> boards = new List<Board>
            // {
            //     new Board {Id = list.Id,
            //         Subject = list.Subject,
            //         UserName = list.UserName}
            // };


            // List<BoardDetailsViewModel> bdvm = new List<BoardDetailsViewModel>();


            // foreach (Board boardss in boards)
            // {
            //     BoardDetailsViewModel bd = new BoardDetailsViewModel();

            //     bd.board.Subject = boardss.Subject;
            //     bd.board.UserName = boardss.UserName;

            //     bdvm.Add(bd);
            // }

            //var list = await _context.Board.FirstOrDefaultAsync(m => m.Id == id);
            //var md = new List<Board>
            //{
            //    new Board { Content = list.Content}
            //};



         //   return View(bd);
        //}


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
