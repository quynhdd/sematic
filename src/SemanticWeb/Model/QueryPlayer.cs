using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemanticWeb.Model
{
    public class QueryPlayerRequest
    {
        public string playerName { get; set; }
    }
    public class DataQuery
    {
        public QueryPlayerModel Values { get; set; }
        public string[] Keys { get; set; }
    }
    public class QueryPlayerModel
    {
        public string playerName { get; set; }
        public string position { get; set; }
        public string age { get; set; }
        public string image { get; set; }
        public string nationality { get; set; }
    }
}
