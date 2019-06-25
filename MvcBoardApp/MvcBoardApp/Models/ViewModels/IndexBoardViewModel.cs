using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcBoardApp.Models.DTO;

namespace MvcBoardApp.Models.ViewModels
{
    public class IndexBoardViewModel
    {
        public PaginatedList<Board> Boards { get; set; }
      
        public ESortOrder SortOrder { get; set; }        
    }
}
