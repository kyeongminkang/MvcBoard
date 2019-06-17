using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcBoardApp.Models;

namespace MvcBoardApp.Models.ViewModels
{
    public class BoardViewModel
    {
        public int PageIndex { get; set; }

        public Board Board { get; set; }

        public List<Comment> Comments { get; set; }
      
    }
}
