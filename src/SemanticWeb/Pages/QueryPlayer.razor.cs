using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using Newtonsoft.Json;
using SemanticWeb.Data;
using SemanticWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemanticWeb.Pages
{
    public class QueryPlayerBase:Session
    {
        private static ILogger<QueryPlayerBase> _logger;
        private static IDriver _driver;
        public QueryPlayerModel query = new QueryPlayerModel();
        public QueryPlayerModel query1 = new QueryPlayerModel();
        public class Data
        {
            public string value { get; set; }
            public string text { get; set; }
        }
        public List<Data> data = new List<Data>();
        public static void Configure(ILogger<QueryPlayerBase> logger, IDriver driver)
        {
            _logger = logger;
            _driver = driver;
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                data = await GetDataDropDown();
            }
            StateHasChanged();
        }
        public async Task changePlayer(string value)
        {
            List<QueryPlayerModel> ttf = new List<QueryPlayerModel>();
            ttf = await GetDataQuery(value);
            if(ttf.Count == 0)
            {
                query1.playerName = query1.position = query1.age = query1.nationality = "Không có dữ liệu"!;
                query1.image = "";
            }
            else
            {
                query1 = ttf.FirstOrDefault();
            }
            StateHasChanged();
        }
        public async Task<List<Data>> GetDataDropDown()
        {
            List<Data> ttf = new List<Data>();
            IResultCursor cursor;
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                cursor = await session.RunAsync(@"match(p:Player) return p.name as playerName,p.position as position,p.age as age,p.image as image, p.nationality as nationality;");
                var a = await cursor.ToListAsync();
                var aa = JsonConvert.SerializeObject(a);

                List<DataQuery> dt = new List<DataQuery>();
                dt = (List<DataQuery>)JsonConvert.DeserializeObject(aa, typeof(List<DataQuery>));

                foreach (var item in dt)
                {
                    ttf.Add(new Data { value = item.Values.playerName, text = item.Values.playerName });
                }
            }
            catch (Exception ex)
            {

            }

            return ttf;
        }
        public async Task<List<QueryPlayerModel>> GetDataQuery(string value)
        {
            List<QueryPlayerModel> ttf = new List<QueryPlayerModel>();
            IResultCursor cursor;
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                cursor = await session.RunAsync(@"match(p:Player) where p.name='" + value + "' return p.name as playerName,p.position as position,p.age as age,p.image as image, p.nationality as nationality;");
                var a = await cursor.ToListAsync();
                var aa = JsonConvert.SerializeObject(a);

                List<DataQuery> dt = new List<DataQuery>();
                dt = (List<DataQuery>)JsonConvert.DeserializeObject(aa, typeof(List<DataQuery>));

                foreach (var item in dt)
                {
                    ttf.Add(new QueryPlayerModel { playerName = item.Values.playerName, position = item.Values.position, age = item.Values.age, image = item.Values.image, nationality = item.Values.nationality });
                }
            }
            catch (Exception ex)
            {

            }

            return ttf;
        }
    }
}
