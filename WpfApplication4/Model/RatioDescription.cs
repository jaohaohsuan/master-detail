using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReactiveUI;
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

        public static T ConvertTo<T>(this string json)
        {
            var jObj = JsonObject.Parse(json);

            var type = TypesImplementingInterface(typeof(T)).FirstOrDefault(o => PropertyAllMatched(o, jObj));

            var obj = typeof(StringExtensions)
                         .GetMethod("FromJson", BindingFlags.Static | BindingFlags.Public)
                         .MakeGenericMethod(type)
                         .Invoke(null, new object[] { json });

            return (T)obj;
        }

        private static bool PropertyAllMatched(Type type, JsonObject obj)
        {
            var a = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(o => o.Name).ToArray();
            var b = obj.Keys;

            var result = a.Union(b).Count() == a.Count();
            return result;
        }
    }

    public class RatioDescription : ReactiveObject, IEvaluationItemDescription
    {
        private string _Denominator;
        private string _Numerator;
        private string _Unit;

        public string Denominator
        {
            get { return _Denominator; }
            set { this.RaiseAndSetIfChanged(x => x.Denominator, value); }
        }

        public string Numerator
        {
            get { return _Numerator; }
            set { this.RaiseAndSetIfChanged(x => x.Numerator, value); }
        }

        public string Unit { get { return _Unit; } set { this.RaiseAndSetIfChanged(x => x.Unit, value); } }
    }
}