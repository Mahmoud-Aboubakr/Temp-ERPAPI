﻿namespace Api;

public class PaginationParams
{
    private const int MAX_PAGE_SIZE = 500;

    public int PageNumber { get; set; } = 1;

    private int pageSize = 25;
    public string Term { get; set; } = ""; 
    public int PageSize
    {
        get { return pageSize; }
        set { pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value; }
    }
}
