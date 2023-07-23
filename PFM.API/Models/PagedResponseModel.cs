﻿namespace PFM.API.Models
{
    public class PagedResponseModel<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
      
        public IEnumerable<T> Items { get; set; }
    }
}
