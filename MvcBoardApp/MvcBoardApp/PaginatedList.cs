﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcBoardApp.Models;


namespace MvcBoardApp
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public int startPage { get; private set; }
        public int endPage { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);


            startPage = ((pageIndex - 1) / pageSize) * pageSize + 1;
            endPage = startPage + pageSize - 1;
            if (endPage > TotalPages)
            {
                endPage = TotalPages;
            }

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public Board board { get; set; }

        

        //internal static object CreateAsync(int? v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}