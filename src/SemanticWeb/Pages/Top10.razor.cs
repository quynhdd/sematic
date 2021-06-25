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
    public class Top10Base : Session
    {
        private static ILogger<Top10Base> _logger;
        private static IDriver _driver;
        public RadzenGrid<Top10Tranfers> grv_transfer;
        public List<Top10Tranfers> t10 = new List<Top10Tranfers>();
        public static void Configure(ILogger<Top10Base> logger, IDriver driver)
        {
            _logger = logger;
            _driver = driver;
        }
        public string name = "";
        protected override async Task OnInitializedAsync()
        {
            t10 = await GetData();
        }
        public async Task<List<Top10Tranfers>> GetData()
        {
            List<Top10Tranfers> ttf = new List<Top10Tranfers>();
            IResultCursor cursor;
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                cursor = await session.RunAsync(@"MATCH (t:Transfer)-[:OF_PLAYER]->(player), 
                                                      (from)<-[:FROM_CLUB]-(t)-[:TO_CLUB]->(to)
                                                RETURN player.name as namePlayer, from.name as fromClub, to.name as toClub,
                                                       apoc.number.format(t.value) AS price
                                                ORDER BY t.value DESC
                                                LIMIT 10");
                var a = await cursor.ToListAsync();
                var aa = JsonConvert.SerializeObject(a);

                List<dataTranfer> dt = new List<dataTranfer>();
                dt = (List<dataTranfer>)JsonConvert.DeserializeObject(aa, typeof(List<dataTranfer>));

                foreach (var item in dt)
                {
                    ttf.Add(new Top10Tranfers { namePlayer = item.Values.namePlayer, fromClub = item.Values.fromClub, toClub = item.Values.toClub, price = item.Values.price });
                }
            }
            catch (Exception ex)
            {

            }

            return ttf;
        }
    }
}
