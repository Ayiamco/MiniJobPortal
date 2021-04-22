using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inSpark.Dtos
{
    public class PagedData<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int CurrentPage {get;set;}
        public bool HasMoreData {get;set;}
    }
}