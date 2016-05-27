using System.Text;
namespace JinRi.Fx.Entity.DD
{
    public class Server
    {
        public string ServerName { get; set; }
        public string ServerAddress { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    var sb = new StringBuilder();
                    sb.Append("Data Source=");
                    sb.Append(ServerAddress);
                    sb.Append(";Initial Catalog=master;User ID=");
                    sb.Append(Account);
                    sb.Append(";Password=");
                    sb.Append(Password);
                    sb.Append(";pooling=true;");
                    _connectionString = sb.ToString();
                }
                return _connectionString;
            }
            set { _connectionString = value; }
        }

        private string _connectionString;
    }
}
