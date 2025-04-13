﻿namespace MovieHub.Application.Common;

public class PagedResult<T>
{
    public List<T> Items { get; }
    public int TotalCount { get; }
    public int Page { get; }      
    public int PageSize { get; }   
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize); // Всего страниц

    public PagedResult(List<T> items, int totalCount, int page, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }
}