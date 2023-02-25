using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorCommon.Grid
{
   

    public class GridColumnBase
    {
        public string Name { get; set; }        
        public int Position { get; set; }
        public bool ShowKeyColumn { get; set; }
        public bool KeyColumn { get; set; }   
        public string KeyColumnName { get; set; }
        public PropertyInfo PropertyInfo { get; set; }     
        public string SortSymbol { get; set; }
        public PropertyType PropertyType { get; set; }
        public GridSearch GridSearch { get; set; }
        public bool Searchable { get; set; }
        public bool Sortable { get; set; }

        public static object GetKeyValue(object obj, PropertyInfo prop)
        {
            return prop.GetValue(obj, null);
        }
        public GridColumnBase(PropertyInfo prop, int position, string keyColumnName, bool searchable =true, bool sortable= true)
        {
            PropertyInfo = prop;
            Position = position;
            KeyColumnName = keyColumnName;
            Name = prop.GetCustomAttribute<DisplayAttribute>()?.Name ?? prop.Name;
            KeyColumn = keyColumnName == prop.Name && !ShowKeyColumn;
            PropertyType = GetPropertyType();
            Searchable= searchable;
            Sortable= sortable;            
        }

       

        private PropertyType GetPropertyType()
        {
            if (PropertyInfo !=null)
            {
                if (IsNumericType(PropertyInfo))
                {
                    return PropertyType.number;
                }
                string result = PropertyInfo.GetCustomAttributesData()
                    .FirstOrDefault(x => x.AttributeType.Name.Contains("ColumnAttribute"))?
                    .NamedArguments?.FirstOrDefault().TypedValue.Value.ToString() ??
                 PropertyInfo.PropertyType.Name.ToLower();

                result = result == "string" ? "text" : result;
                PropertyType searchType = (PropertyType)Enum.Parse(typeof(PropertyType), result);
                return searchType;
            }
            return PropertyType.noType;
        }

        private bool IsNumericType(PropertyInfo prop)
        {
            TypeCode tt = (TypeCode)Enum.Parse(typeof(TypeCode), prop.PropertyType.Name);
            switch (tt)
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
