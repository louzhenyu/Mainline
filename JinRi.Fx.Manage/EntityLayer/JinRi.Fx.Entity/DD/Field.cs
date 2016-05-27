namespace JinRi.Fx.Entity.DD
{
    public class Field
    {
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public int FieldOrder { get; set; }
        public string FieldDescription { get; set; }
        public string FieldType { get; set; }
        public int FieldMaxLength { get; set; }
        public bool IsFieldAutoIncrement { get; set; }
        public bool IsFieldPrimaryKey { get; set; }
        public bool IsFieldNullable { get; set; }
        public string FieldDefaultValue { get; set; }
        public int FieldPosition { get; set; }
    }
}
