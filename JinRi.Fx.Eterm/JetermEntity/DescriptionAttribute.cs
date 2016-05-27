using System;

namespace JetermEntity
{
    public class DescriptionAttribute:Attribute
    {
        public DescriptionAttribute(string desc)
        {
            Description = desc;
        }

        public string Description { get; set; }

        public static string GetEnumDescrition<TEnum>(object value)
        {
            if (value == null) return string.Empty;

            Type enumType = typeof(TEnum);

            if (!enumType.IsEnum) return string.Empty;

            string name = Enum.GetName(enumType, value);

            if (string.IsNullOrEmpty(name)) return string.Empty;

            object[] objs = enumType.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (objs == null || objs.Length == 0) return string.Empty;

            return (objs[0] as DescriptionAttribute).Description;
        }
    }
}
