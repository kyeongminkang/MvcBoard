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

        public Board board { get; set; }
    }
}
