﻿namespace PFM.API.Models
{
    public class AutoCategorizeRule
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CatCode { get; set; }

        public string Predicate { get; set; }

    }
}
