using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcBoardApp.Models
{
    public class Board
    {
        public int Id { get; set; }
 
        public string UserName { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        [DataType(DataType.Date)]
        public DateTime WriteDate { get; set; }

        public int CommentCount { get; set; }
    }
}
