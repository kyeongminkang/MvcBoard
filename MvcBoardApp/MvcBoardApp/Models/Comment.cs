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
        public int Id { get; set; }

        [ForeignKey("Id")]
        public int BoardId { get; set; }

        public string CommentUserName { get; set; }

        public string CommentContent { get; set; }

        public Board Board { get; set; }
    }
}
