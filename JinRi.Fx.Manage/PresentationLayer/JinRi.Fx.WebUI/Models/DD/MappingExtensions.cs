using System.Collections.Generic;
using AutoMapper;
using JinRi.Fx.Entity.DD;

namespace JinRi.Fx.WebUI.Models.DD
{
    public static class MappingExtensions
    {
        static MappingExtensions()
        {
            Mapper.CreateMap<Database, DatabaseModel>();
            Mapper.CreateMap<DatabaseModel, Database>();
            Mapper.CreateMap<Table, TableModel>();
            Mapper.CreateMap<TableModel, Table>();
            Mapper.CreateMap<Field, FieldModel>();
            Mapper.CreateMap<FieldModel, Field>();
            Mapper.CreateMap<Log, LogModel>();
            Mapper.CreateMap<LogModel, Log>();
        }
        public static DatabaseModel ToDatabaseModel(this Database database)
        {
            return Mapper.Map<DatabaseModel>(database);
        }
        public static Database ToDatabase(this DatabaseModel databaseModel)
        {
            return Mapper.Map<Database>(databaseModel);
        }
        public static TableModel ToTableModel(this Table table)
        {
            return Mapper.Map<TableModel>(table);
        }
        public static Table ToTable(this TableModel tableModel)
        {
            return Mapper.Map<Table>(tableModel);
        }
        public static FieldModel ToFieldModel(this Field field)
        {
            return Mapper.Map<FieldModel>(field);
        }
        public static Field ToField(this FieldModel fieldModel)
        {
            return Mapper.Map<Field>(fieldModel);
        }
        public static IEnumerable<DatabaseModel> ToDatabaseModels(this IEnumerable<Database> databases)
        {
            return Mapper.Map<IEnumerable<DatabaseModel>>(databases);
        }
        public static IEnumerable<TableModel> ToTableModels(this IEnumerable<Table> tables)
        {
            return Mapper.Map<IEnumerable<TableModel>>(tables);
        }
        public static IEnumerable<FieldModel> ToFieldModels(this IEnumerable<Field> fields)
        {
            return Mapper.Map<IEnumerable<FieldModel>>(fields);
        }
        public static Log ToLog(this LogModel logModel)
        {
            return Mapper.Map<Log>(logModel);
        }
        public static IEnumerable<LogModel> ToLogModels(this IEnumerable<Log> logs)
        {
            return Mapper.Map<IEnumerable<LogModel>>(logs);
        }
    }
}