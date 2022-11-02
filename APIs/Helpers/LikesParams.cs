using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIs.Helpers
{
    public class LikesParams : PaginationParams
    {
        public int UserId { get; set; }
        public string  Predicates { get; set; }
    }
}