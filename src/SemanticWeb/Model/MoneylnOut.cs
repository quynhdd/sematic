using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemanticWeb.Model
{
    public class dataMoney
    {
        public MoneylnOut Values { get; set; }
        public string[] Keys { get; set; }
    }
    public class MoneylnOut
    {
        public string clubName { get; set; }
        public string inMoney { get; set; }
        public string outMoney { get; set; }
    }
}
