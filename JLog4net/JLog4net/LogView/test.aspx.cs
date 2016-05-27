using MongoDB.Bson;
using MongoDB.Driver;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogView
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGetOrderCount_Click(object sender, EventArgs e)
        {
            PooledRedisClientManager pooleManager = new PooledRedisClientManager(10, 5, ConfigurationManager.AppSettings["RedisServerIP"].ToString());
            using (var redisClient = pooleManager.GetClient())
            {
               lblGetCount.Text= redisClient.GetListCount("ORDER_NO").ToString();
               
            }
        }

        protected void btnGetConnlections_Click(object sender, EventArgs e)
        {
            BsonDocument docFind = new BsonDocument();
            MongoDB.Driver.MongoServer server = MongoDB.Driver.MongoServer.Create(ConfigurationManager.AppSettings["mongoDBConfig"]);
            server.Connect();
            //获取指定数据库
            MongoDB.Driver.MongoDatabase db = server.GetDatabase(ConfigurationManager.AppSettings["mongoDBName"].ToString());
            string collection = txtCollection.Text.Trim();
           
            //获取表
            MongoDB.Driver.MongoCollection<BsonDocument> col = db.GetCollection<BsonDocument>(collection);
            lblCollections.Text = "表大小:" + col.GetTotalDataSize() / (1024 * 1024) + "M";//db['119004logs'].totalIndexSize()+db['119004logs'].dataSize()
            this.listBoxCollections.DataSource=col.Database.GetCollectionNames();
            listBoxCollections.DataBind();

            lblDataBaseCount.Text = "数据库大小为："+db.GetStats().StorageSize / (1024 * 1024) + "M";

        }

        protected void btnGetIndex_Click(object sender, EventArgs e)
        {
            BsonDocument docFind = new BsonDocument();
            MongoDB.Driver.MongoServer server = MongoDB.Driver.MongoServer.Create(ConfigurationManager.AppSettings["mongoDBConfig"]);
            server.Connect();
            //获取指定数据库
            MongoDB.Driver.MongoDatabase db = server.GetDatabase(ConfigurationManager.AppSettings["mongoDBName"].ToString());
            string collection = txtCollection.Text.Trim();

            //获取表
            MongoDB.Driver.MongoCollection<BsonDocument> col = db.GetCollection<BsonDocument>(collection);
            GetIndexesResult indexsResult=col.GetIndexes();
            string strIndexs = string.Empty;
            for (int i = 0; i < indexsResult.Count;i++ )
            {
                strIndexs += indexsResult[i].Key.ToString()+"；";
            }
            txtGetIndexs.Text = strIndexs;
        }
    }
}