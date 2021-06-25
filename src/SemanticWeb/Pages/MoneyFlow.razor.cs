using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using Newtonsoft.Json;
using Radzen.Blazor;
using SemanticWeb.Data;
using SemanticWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SemanticWeb.Pages
{
    public class MoneyFlowBase:Session
    {
        private static ILogger<MoneyFlowBase> _logger;
        private static IDriver _driver;
        public RadzenGrid<MoneyF> grv_transfer;
        public List<MoneyF> mnf = new List<MoneyF>();
        public static void Configure(ILogger<MoneyFlowBase> logger, IDriver driver)
        {
            _logger = logger;
            _driver = driver;
        }
        public string name = "";
        protected override async Task OnInitializedAsync()
        {
            mnf = await GetData();
        }
        public async Task<List<MoneyF>> GetData()
        {
            List<MoneyF> ttf = new List<MoneyF>();
            IResultCursor cursor;
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                cursor = await session.RunAsync(@"MATCH (t:Transfer)-[:OF_PLAYER]->(player),
                                                      (fromCountry)<-[:IN_COUNTRY]-(fromLeague),
                                                      (fromLeague)<-[:IN_LEAGUE]-(from)<-[:FROM_CLUB]-(t),
                                                      (t)-[:TO_CLUB]->(to)-[:IN_LEAGUE]->(toLeague),
                                                      (toLeague)-[:IN_COUNTRY]->(toCountry)
                                                WITH *
                                                ORDER BY fromLeague, toLeague, t.value DESC
                                                WITH fromLeague, toLeague, sum(t.value) AS totalFees, 
                                                     fromCountry, toCountry, 
                                                     collect({player: player.name, fee: t.value}) AS transfers
                                                WHERE fromCountry <> toCountry
                                                RETURN fromCountry.name as fromCountry, 
                                                       toCountry.name as toCountry, 
                                                       apoc.number.format(totalFees) AS total, 
                                                       transfers[0].player AS player,
                                                       apoc.number.format(transfers[0].fee) AS fee, 
                                                       size(transfers) AS numberOfTransfers
                                                ORDER By totalFees DESC
                                                LIMIT 10");
                var a = await cursor.ToListAsync();
                var aa = JsonConvert.SerializeObject(a);

                List<dataMoneyF> dt = new List<dataMoneyF>();
                dt = (List<dataMoneyF>)JsonConvert.DeserializeObject(aa, typeof(List<dataMoneyF>));

                foreach (var item in dt)
                {
                    ttf.Add(new MoneyF { fromCountry = item.Values.fromCountry, toCountry = item.Values.toCountry, total = item.Values.total, player = item.Values.player, fee = item.Values.fee, numberOfTransfers = item.Values.numberOfTransfers });
                }
            }
            catch (Exception ex)
            {

            }

            return ttf;
        }
    }
}
