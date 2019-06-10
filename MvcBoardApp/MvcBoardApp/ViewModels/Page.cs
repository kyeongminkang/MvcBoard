using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcBoardApp.Models;

namespace MvcBoardApp.ViewModels
{
    public class Page
    {
        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public Board board { get; set; }


        public List<Comment> Comments { get; set; }

        public Comment comment { get; set; }
    }
}
