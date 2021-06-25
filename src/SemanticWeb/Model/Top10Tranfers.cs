using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemanticWeb.Model
{
    public class dataTranfer
    {
        public Top10Tranfers Values { get; set; }
        public string[] Keys { get; set; }
    }
    public class Top10Tranfers
    {
        public string namePlayer { get; set; }
        public string fromClub { get; set; }
        public string toClub { get; set; }
        public string price { get; set; }
    }
}
