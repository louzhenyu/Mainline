namespace JinRi.Fx.WebUI.Models.DD
{
    public class TableModel
    {
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string TableOwner { get; set; }
        public string TableDescription { get; set; }
        public int ColCount { get; set; }
    }
}