using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcBoardApp.Models.DTO;
using MvcBoardApp.Models.ViewModels;
using MvcBoardApp;


namespace MvcBoardApp.Models.ViewModels
{
    public class IndexBoardViewModel
    {
        public PaginatedList<Board> Boards { get; set; }
       
        public string SortOrder { get; set; }
        
    }
}
