﻿namespace Udemy.Areas.manage.ViewModels
{
    public class PaginatedList<T>
    {
        public PaginatedList(List<T> items, int pageIndex, int totalPage,int size)
        {
            Items = items;
            PageIndex = pageIndex;
            TotalPage = totalPage;
            Size = size;
        }

        public List<T> Items { get; }
        public int PageIndex { get; }
        public int TotalPage { get; }

        public int Size { get; }
        public bool HasNext => PageIndex < TotalPage;
        public bool HasPrev => PageIndex > 1;


        public static PaginatedList<T> Create(IQueryable<T> query, int page, int size)
        {
            int total = (int)Math.Ceiling(query.Count() / (double)size);
            return new PaginatedList<T>(query.Skip((page - 1) * size).Take(size).ToList(), page, total,size);
        }
    }
}
