using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcBoardApp.Models;

namespace MvcBoardApp.Models.ViewModels
{
    public class CommentViewModel
    {
        public int PageIndex { get; set; }

        public Comment Comment { get; set; }
    }
}
