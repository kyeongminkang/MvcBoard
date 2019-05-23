using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MvcBoardApp.Models
{
    public class Board
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Required]
        public string Contents { get; set; }
    }
}
