using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcBoardApp.Models;


namespace MvcBoardApp.Models.ViewModels
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public int StartPage { get; private set; }
        public int EndPage { get; private set; }

        public string SortOrder { get; set; }
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize, string sortOrder)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            StartPage = ((pageIndex - 1) / pageSize) * pageSize + 1;
            EndPage = StartPage + pageSize - 1;
            if (EndPage > TotalPages)
            {
                EndPage = TotalPages;
            }

            SortOrder = sortOrder;

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

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize, string sortOrder)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize, sortOrder);
        }
   

        
    }
}
