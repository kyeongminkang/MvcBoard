using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcBoardApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MvcBoardApp.Controllers;




namespace MvcBoardApp.ViewModels
{


    public class BoardDetailsViewModel

    {
        //public string memo { get; set; }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Subject { get; set; }

        public string Content { get; set; }
        public string C_UserName { get; set; }

        public string C_Content { get; set; }


        public int BoardId { get; set; }
        //public Board board { get; set; }

        //public Comment comment { get; set; }


        public List<Comment> comments { get; set; }
        public Board board { get; set; }




        public Comment comment { get; set; }

      //  public BoardCommentModel boardComment { get; set; }
       //public BoardCommentModel boardComentModel { get; set; }

//        public IEnumerable<BoardCommentModel> boardCommentModel { get; set; }


        public IEnumerable<Board> BoardObject { get; set; }
        public IEnumerable<Comment> CommentObject { get; set; }

    }



    
   


}
