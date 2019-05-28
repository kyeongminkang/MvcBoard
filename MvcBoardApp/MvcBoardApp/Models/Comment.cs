using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcBoardApp.Models
{
    public class Comment
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ForeignKey("Id")]
        public int BoardId { get; set; }

        public string UserName { get; set; }

        public string content { get; set; }

        public Board board { get; set; }

    }
}
