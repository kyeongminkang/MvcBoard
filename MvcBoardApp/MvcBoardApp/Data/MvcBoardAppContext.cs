﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcBoardApp.Models;

namespace MvcBoardApp.Models
{
    public class MvcBoardAppContext : DbContext
    {
        public MvcBoardAppContext (DbContextOptions<MvcBoardAppContext> options)
            : base(options)
        {
        }

        public DbSet<MvcBoardApp.Models.Board> Board { get; set; }

        public DbSet<MvcBoardApp.Models.Member> Member { get; set; }
    }
}
