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
        public string Title { get; set; }
        [Required]
        public string Contents { get; set; }
        
       //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
       // public DateTime WriteTime { get; set; }
    }
}
