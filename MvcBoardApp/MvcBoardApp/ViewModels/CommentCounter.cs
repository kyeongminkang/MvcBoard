using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcBoardApp.Models;

namespace MvcBoardApp.ViewModels
{
    public class CommentCounter
    {
        public int? ID { get; set; }

        public int? CommentCount { get; set; }

        public void GetCount(Board board)
        {
            board.CommentCount = (int)CommentCount;
        }
    }
}
