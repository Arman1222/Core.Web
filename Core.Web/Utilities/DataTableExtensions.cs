using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Core.Web.Utilities
{

    //http://stackoverflow.com/questions/17107220/convert-dataset-to-list
    public static class DataTableExtensions
    {        
            public static List<T> ToList<T>(this DataTable table) where T : new()
            {
                IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
                List<T> result = new List<T>();

                foreach (var row in table.Rows)
                {
                    var item = CreateItemFromRow<T>((DataRow)row, properties);
                    result.Add(item);
                }

                return result;
            }
            
            private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
            {
                T item = new T();
                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(System.DayOfWeek))
                    {
                        DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), row[property.Name].ToString());
                        property.SetValue(item, day, null);
                    }
                    else
                    {                        
                        if (row.Table.Columns.Contains(property.Name) &&
                            row[property.Name] != DBNull.Value                            
                            ) //jika column dari datarow ada
                        {
                            property.SetValue(item, row[property.Name], null);
                        }
                    }
                }
                return item;
            }
            //http://stackoverflow.com/questions/18100783/how-to-convert-a-list-into-data-table
            public static DataTable ToDataTable<T>(this List<T> items)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);

                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
                //put a breakpoint here and check datatable
                return dataTable;
            }
    }
}
