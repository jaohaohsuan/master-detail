using ReactiveUI;
using ServiceStack.ServiceModel.Serialization;

namespace WpfApplication4.Model
{
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