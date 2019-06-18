using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MvcBoardApp.Models.ViewModels
{
    public class EditCommentViewModel
    {
        public int PageIndex { get; set; }

        [Required]
        public int ID { get; set; }

        [Required]
        public int BoardID { get; set; }

        [Required]
        public string CommentUserName { get; set; }

        [Required]
        [StringLength(300)]
        public string CommentContent { get; set; }
    }
}
