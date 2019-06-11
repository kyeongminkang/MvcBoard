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
        public int? ID { get; set; }

        [ForeignKey("ID")]
        public int BoardID { get; set; }

        [Required]
        public string CommentUserName { get; set; }

        [Required]
        public string CommentContent { get; set; }

        public Board Board { get; set; }
    }
}
