using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcBoardApp.Models;

namespace MvcBoardApp.ViewModels
{
    public class BoardComment
    {
        public Board board { get; set; }
        public List<Comment> comment { get; set; }
    }
}
