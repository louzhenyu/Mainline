using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.Impl;

namespace SolrDemo
{
    internal class SolrNetApi
    {
        private static void Main(string[] args)
        {
            Startup.Init<SolrEntityOrder>("http://192.168.2.196:8983/solr/OrderNoSearcher");
            //Delete();
            Add();
            //Query(1);
            Console.WriteLine("操作已经完成，按任意键退出。");
            Console.ReadKey();
        }

        //删
        public static void Delete()
        {
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<SolrEntityOrder>>();
            solr.Delete(SolrQuery.All);
            solr.Commit();
        }

        //增，改, 准实时数据导入
        public static void Add()
        {
            var p = new SolrEntityOrder
            {
                OrderId = 558695,
                //OrderNo = "0809010805542817910543",
                //OrderTime = DateTime.Now,
                //PassengerName = "爱新觉罗张三",
                //PNR = "NQSNQSNQS",
                //TicketNo = "859-890420888090",
                //SolrUpdateTime = DateTime.Now
            };

            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<SolrEntityOrder>>();
            solr.Add(p);
            solr.Commit();
        }

        //查
        public static void Query(int queryType)
        {
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<SolrEntityOrder>>();
            SolrQueryResults<SolrEntityOrder> results = null;
             
            var queryOptions = new QueryOptions
            {
                StartOrCursor = new StartOrCursor.Start(0),
                Rows = 20
            };
            switch (queryType)
            {
                case 1:
                    //简单查询
                    results = solr.Query(new SolrQuery("PassengerName:\"李光第一\""), queryOptions);
                    break;
                case 2:
                    //字段查询
                    results = solr.Query(new SolrQueryByField("PassengerName", "晓"), queryOptions);
                    break;
                case 3:
                    //范围查询
                    results =
                        solr.Query(
                            new SolrQueryByRange<DateTime>("OrderTime", new DateTime(2008, 4, 15),
                                new DateTime(2008, 4, 16)), queryOptions);
                    break;
                case 4:
                    //多值查询
                    results = solr.Query(new SolrQueryInList("PassengerName", "陶", "仝", "胡"), queryOptions);
                    break;
                case 5:
                    //任意值查询
                    results = solr.Query(new SolrHasValueQuery("PassengerName"), queryOptions);
                    break;
                case 6:
                    //组合查询
                    results = solr.Query(new SolrQuery("PassengerName:\"李晓\"") && new SolrQuery("OrderNo:\"2013012\""),
                        queryOptions);
                    break;
                case 7:
                    //组合查询
                    results = solr.Query(new SolrQuery("PassengerName:\"李晓\"") || new SolrQuery("OrderNo:\"2013012\""),
                        queryOptions);
                    break;
                case 8:
                    //组合查询
                    results = solr.Query(new SolrQuery("PassengerName:\"李晓\"") + new SolrQuery("OrderNo:\"2013012\""),
                        queryOptions);
                    break;
                case 9:
                    //组合查询
                    results = solr.Query(new SolrQuery("OrderNo:\"2013012\"") - new SolrQuery("PassengerName:\"李晓\""),
                        queryOptions);
                    break;
                case 10:
                    //组合查询
                    results = solr.Query(new SolrQuery("OrderNo:\"2013012\"") + !new SolrQuery("PassengerName:\"李晓\""),
                        queryOptions);
                    break;
                case 11:
                    //过滤查询
                    results = solr.Query(SolrQuery.All, new QueryOptions
                    {
                        FilterQueries = new ISolrQuery[]
                        {
                            new SolrQueryByField("PassengerName", "\"张三\""),
                            new SolrQueryByRange<DateTime>("OrderTime", new DateTime(2008, 4, 15),
                                new DateTime(2008, 4, 16))
                        },
                        StartOrCursor = new StartOrCursor.Start(0),
                        Rows = 20
                    });
                    break;
                case 12:
                    //返回字段
                    results = solr.Query(SolrQuery.All, new QueryOptions
                    {
                        Fields = new[] {"PassengerName", "TicketNo"},
                        StartOrCursor = new StartOrCursor.Start(0),
                        Rows = 20
                    });
                    break;
                case 13:
                    //排序
                    results = solr.Query(SolrQuery.All, new QueryOptions
                    {
                        OrderBy = new[] {new SortOrder("OrderTime", Order.DESC), SortOrder.Parse("UpdateTime asc")},
                        StartOrCursor = new StartOrCursor.Start(0),
                        Rows = 20
                    });
                    break;
                case 14:
                    //分页
                    results = solr.Query(SolrQuery.All, new QueryOptions
                    {
                        StartOrCursor = new StartOrCursor.Start(10),
                        Rows = 25
                    });
                    break;
                default:
                    //简单查询
                    results = solr.Query(new SolrQuery("PassengerName:\"李晓\""), queryOptions);
                    break;
            }

            Console.WriteLine("查询结果：");
            foreach (SolrEntityOrder i in results)
            {
                Console.WriteLine(i.PassengerName);
            }
        }
    }
}