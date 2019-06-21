using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcBoardApp.Models.DTO;
using MvcBoardApp.Models.ViewModels;
using MvcBoardApp;
using static MvcBoardApp.Models.DTO.ESrotOrder;

namespace MvcBoardApp.Models.ViewModels
{
    public class IndexBoardViewModel
    {
        public PaginatedList<Board> Boards { get; set; }
      
        public ESortOrder EnumSortOrder { get; set; }        
    }
}
