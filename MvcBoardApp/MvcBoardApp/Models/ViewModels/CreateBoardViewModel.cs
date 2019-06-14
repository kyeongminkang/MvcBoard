using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcBoardApp.Models.ViewModels
{
    public class CreateBoardViewModel
    {
        public int PageIndex { get; set; }

        public Board Board { get; set; }
    }
}
