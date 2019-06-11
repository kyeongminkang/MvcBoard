using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcBoardApp.Models;

namespace MvcBoardApp.ViewModels
{
    public class BoardComment
    {
        public int PageIndex { get; set; }

        public Board Board { get; set; }

        public Comment Comment { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
