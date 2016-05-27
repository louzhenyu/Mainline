using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Dapper;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Attributes;
using SolrNet.Commands.Parameters;

namespace SolrDemo
{
    class SOLRvsDB
    {
        static void Main1(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            OrderSolrQuery();
            sw.Stop();
            Console.WriteLine("Solr查询耗时毫秒数：" + sw.ElapsedMilliseconds);
            sw.Restart();
            OrderDbQuery();
            sw.Stop();
            Console.WriteLine("数据库查询耗时毫秒数：" + sw.ElapsedMilliseconds);
            Console.ReadKey();
        }

        public static void OrderSolrQuery()
        {
            const string queryType = "PassengerName";
            const string queryValue = "李四";
            var orderBeginTime = new DateTime(1980, 1, 1);
            var orderEndTime = new DateTime(2015, 9, 30);
            const int maxResultCount = 200;

            Startup.Init<SolrEntityOrder>("http://192.168.2.196:8983/solr/OrderNoSearcher");
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<SolrEntityOrder>>();
            
            const string queryString = queryType + ":\"" + queryValue + "\"";
            var queryOptions = new QueryOptions
            {
                FilterQueries = new ISolrQuery[]
                {
                    new SolrQueryByRange<DateTime>("OrderTime", orderBeginTime, orderEndTime)
                },
                Fields = new[] { "OrderNo" },
                OrderBy = new[] { new SolrNet.SortOrder(queryType, Order.DESC), SolrNet.SortOrder.Parse("OrderTime desc") },
                StartOrCursor = new StartOrCursor.Start(0),
                Rows = maxResultCount
            };
            var orderNos = solr.Query(new SolrQuery(queryString), queryOptions);
            Console.WriteLine("Solr查询返回结果数：" + orderNos.Count);
        }
        public static void OrderDbQuery()
        {
            const string queryType = "PassengerName";
            const string queryValue = "李四";
            var orderBeginTime = new DateTime(1980, 1, 1);
            var orderEndTime = new DateTime(2015, 9, 30);
            const int maxResultCount = 200;

            string sql = "SELECT TOP " + maxResultCount + " OrderNo FROM tblOrders WITH(NOLOCK) WHERE " + queryType + " LIKE " + "'%" + queryValue + "%' and ordertime between '" + orderBeginTime + "' and '" + orderEndTime + "' " + "ORDER BY OrderTime DESC";
            using (var conn = new SqlConnection("Data Source=192.168.2.180;Initial Catalog=JinRi;User ID=sa;Password=test;pooling=true;Connect Timeout=300"))
            {
                conn.Open();
                var orderNos = conn.Query<string>(sql, commandTimeout:300);
                Console.WriteLine("数据库查询返回结果数：" + orderNos.Count());
            }
        }
    }

    public class SolrEntityOrder
    {
        [SolrUniqueKey("OrderID")]
        public int OrderId { get; set; }
        [SolrField("PNR")]
        public string PNR { get; set; }
        [SolrField("PassengerName")]
        public string PassengerName { get; set; }
        [SolrField("OrderNo")]
        public string OrderNo { get; set; }
        [SolrField("TicketNo")]
        public string TicketNo { get; set; }
        [SolrField("OrderTime")]
        public DateTime OrderTime { get; set; }
        [SolrField("SolrUpdateTime")]
        public DateTime SolrUpdateTime { get; set; }
    }
}
