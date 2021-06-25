using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemanticWeb.Model
{
    public class dataMoneyF
    {
        public MoneyF Values { get; set; }
        public string[] Keys { get; set; }
    }
    public class MoneyF
    {
        public string fromCountry { get; set; }
        public string toCountry { get; set; }
        public string total { get; set; }
        public string player { get; set; }
        public string fee { get; set; }
        public int numberOfTransfers { get; set; }
    }
}
