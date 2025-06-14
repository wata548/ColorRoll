using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;

namespace CSVData {
    public static partial class CSV {

        public static string Serialize<T>(T datas) where T : IEnumerable {

            IEnumerable list;
            Type targetType;
            if (datas is IDictionary dict) {

                targetType = dict
                    .GetEnumerator()
                    .GetType()
                    .GetGenericArguments()[1];
                var listType = typeof(List<>).MakeGenericType(targetType);
                list = Activator.CreateInstance(listType) as IEnumerable;
                var addMethod = listType.GetMethod("Add", new[]{targetType})!;
                PropertyInfo property = null;
                
                foreach (var data in dict) {
                    property ??= data.GetType().GetProperty("Value")!;
                    addMethod.Invoke(list, new []{property.GetValue(data)});
                }
            }
            else {
                list = datas;
                targetType = typeof(T).GetGenericArguments()[0];
            }
         
            return SerializeList(list, targetType);
        }

        private static string SerializeList<T>(T datas, Type type) where T: IEnumerable {
            var result = new StringBuilder();
            var defaultFlag = BindingFlags.Instance
                              | BindingFlags.Public
                              | BindingFlags.NonPublic
                              | BindingFlags.FlattenHierarchy;
            
            var fields = type.GetFields(defaultFlag)
                .Where(field => field.GetCustomAttribute(typeof(IgnoreSerializeCSVAttribute)) == null)
                .Select(field => (field: field, toString: field.FieldType.GetMethod("ToString", Type.EmptyTypes)));
            var properties = type.GetProperties(defaultFlag)
                .Where(property => 
                    property.CanWrite 
                    && property.GetCustomAttribute(typeof(IgnoreSerializeCSVAttribute)) == null
                )
                .Select(property => (property: property, toString: property.PropertyType.GetMethod("ToString", Type.EmptyTypes)));
            
            var typeNames = new StringBuilder();
            var isFirst = true;
            foreach (var field in fields) {
                if (isFirst) {
                                
                    result.Append($"\"{field.field.Name.Replace("\"", "\"\"")}\"");
                    typeNames.Append($"\"{field.field.FieldType.ToString().Replace("\"", "\"\"")}\"");
                    isFirst = false;
                }
                else {
                                
                    result.Append($",\"{field.field.Name.Replace("\"", "\"\"")}\"");
                    typeNames.Append($",\"{field.field.FieldType.ToString().Replace("\"", "\"\"")}\"");
                }
            }
            foreach (var property in properties) {
                if (isFirst) {
                                
                    result.Append($"\"{property.property.Name.Replace("\"", "\"\"")}\"");
                    typeNames.Append($",\"{property.property.PropertyType.ToString().Replace("\"", "\"\"")}\"");
                    isFirst = false;
                }
                else {
                                
                    result.Append($",\"{property.property.Name.Replace("\"", "\"\"")}\"");
                    typeNames.Append($",\"{property.property.PropertyType.ToString().Replace("\"", "\"\"")}\"");
                }
            }

            result.Append('\n');
            result.Append(typeNames.ToString());
                        
            foreach (var data in datas) {
                result.Append("\n");
                isFirst = true;
                foreach (var field in fields) {
                    string value = (string)field.toString!
                        .Invoke(field.field.GetValue(data), new object[] {});  
                                
                    if (isFirst) {
                        result.Append($"\"{value.Replace("\"", "\"\"")}\"");
                        isFirst = false;
                    }
                    else 
                        result.Append($",\"{value.Replace("\"", "\"\"")}\"");
                }
                foreach (var property in properties) {
                    string value = (string)property.toString!
                        .Invoke(property.property.GetValue(data), new object[] {});  
                    if (isFirst) {
                                            
                        result.Append($"\"{value.Replace("\"", "\"\"")}\"");
                        isFirst = false;
                    }
                    else
                        result.Append($",\"{value.Replace("\"", "\"\"")}\"");
                }    
            }
            
            return result.ToString();
        }
    }
}