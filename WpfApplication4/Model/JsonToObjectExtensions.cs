using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ServiceStack.Text;

namespace WpfApplication4.Model
{
    public static class JsonToObjectExtensions
    {
        /// <summary>
        /// Returns all types in the current AppDomain implementing the interface or inheriting the type. 
        /// </summary>
        public static IEnumerable<Type> TypesImplementingInterface(Type desiredType)
        {
            return AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(desiredType.IsAssignableFrom);
        }

        public static T ConvertTo<T>(this IDictionary<string,string> value)
        {
            var json = JsonSerializer.SerializeToString(value);
            var type = TypesImplementingInterface(typeof(T)).FirstOrDefault(o => PropertyAllMatched(o, json));

            if (type == null)
                return default(T);

            var obj = typeof(StringExtensions)
                .GetMethod("FromJson", BindingFlags.Static | BindingFlags.Public)
                .MakeGenericMethod(type)
                .Invoke(null, new object[] { json });

            return (T)obj;
        }

        private static bool PropertyAllMatched(Type type, string json)
        {
            var obj = JsonObject.Parse(json);

            if (obj == null)
                return false;

            var a = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(o => o.Name).ToArray();
            var b = obj.Keys;

            var result = a.Union(b).Count() == a.Count();
            return result;
        }
    }
}