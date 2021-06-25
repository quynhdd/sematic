using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using Newtonsoft.Json;
using Radzen.Blazor;
using SemanticWeb.Data;
using SemanticWeb.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticWeb.Pages
{
    public class MoneyIOBase:Session
    {
        private static ILogger<MoneyIOBase> _logger;
        private static IDriver _driver;
        public RadzenGrid<MoneylnOut> grv_transfer;
        public List<MoneylnOut> mnio = new List<MoneylnOut>();
        public static void Configure(ILogger<MoneyIOBase> logger, IDriver driver)
        {
            _logger = logger;
            _driver = driver;
        }
        public string name = "";
        public class DataItem
        {
            public string Name { get; set; }
            public double Value { get; set; }
        }
        public List<DataItem> MoneyIn = new List<DataItem>();
        public List<DataItem> MoneyOut = new List<DataItem>();
        public string FormatAsUSD(object value)
        {
            return ((double)value).ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));
        }
        protected override async Task OnInitializedAsync()
        {
            mnio = await GetData();
            foreach(var item in mnio)
            {
                MoneyIn.Add(new DataItem { Name = item.clubName, Value = Convert.ToDouble(item.inMoney) });
                MoneyOut.Add(new DataItem { Name = item.clubName, Value = Convert.ToDouble(item.outMoney) });
            }
        }
        public async Task<List<MoneylnOut>> GetData()
        {
            List<MoneylnOut> ttf = new List<MoneylnOut>();
            IResultCursor cursor;
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                cursor = await session.RunAsync(@"MATCH (club:Club)
                                                WITH club,
                                                     apoc.coll.sumLongs(
                                                       [(club)<-[:FROM_CLUB]-(t) | t.value]) AS moneyIn,
                                                     apoc.coll.sumLongs(
                                                       [(club)<-[:TO_CLUB]-(t) | t.value]) AS moneyOut
                                                RETURN club.name as clubName, 
                                                       apoc.number.format(moneyIn) AS inMoney, 
                                                       apoc.number.format(moneyOut) AS outMoney
                                                ORDER BY moneyIn + moneyOut DESC
                                                LIMIT 10");
                var a = await cursor.ToListAsync();
                var aa = JsonConvert.SerializeObject(a);

                List<dataMoney> dt = new List<dataMoney>();
                dt = (List<dataMoney>)JsonConvert.DeserializeObject(aa, typeof(List<dataMoney>));

                foreach (var item in dt)
                {
                    ttf.Add(new MoneylnOut { clubName = item.Values.clubName, inMoney = item.Values.inMoney, outMoney = item.Values.outMoney });
                }
            }
            catch (Exception ex)
            {

            }

            return ttf;
        }
    }
}
