using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ReactiveUI;
using ServiceStack.Text;

namespace WpfApplication4.Model
{
    public interface IEvaluationItemDescription : INotifyPropertyChanged
    {
    }

    public static class EvaluationItemDescription
    {
        public static IEvaluationItemDescription To(this string json)
        {
            JsonObject jObj = JsonObject.Parse(json);

            if (PropertyAllMatched(typeof (RatioDescription), jObj))
            {
                return json.FromJson<RatioDescription>();
            }
            if (PropertyAllMatched(typeof (PieceDescription), jObj))
            {
                return json.FromJson<PieceDescription>();
            }
            return null;
        }

        private static bool PropertyAllMatched(Type type, JsonObject obj)
        {
            var a = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(o => o.Name).ToArray();
            var b = obj.Keys;

            bool result = a.Union(b).Count() == a.Count();
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

    public class PieceDescription : ReactiveObject, IEvaluationItemDescription
    {
        private string _Title;
        private string _Unit;
        public string Title { get { return _Title; } set { this.RaiseAndSetIfChanged(x => x.Title, value); } }

        public string Unit { get { return _Unit; } set { this.RaiseAndSetIfChanged(x => x.Unit, value); } }
    }
}