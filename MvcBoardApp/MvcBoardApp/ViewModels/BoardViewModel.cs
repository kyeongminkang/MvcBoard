﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcBoardApp.Models;

namespace MvcBoardApp.ViewModels
{
    public class BoardViewModel
    {
        public int PageIndex { get; set; }

        public Board Board { get; set; }

        public List<Comment> Comments { get; set; }

        public int? CommentCount { get; set; }

        public void GetCount(Board board)
        {
            board.CommentCount = (int)CommentCount;
        }
    }
}
