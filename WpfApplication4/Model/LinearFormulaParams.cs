using ReactiveUI;

namespace WpfApplication4.Model
{
    public class LinearFormulaParams : ReactiveObject, IFormulaParams
    {
        private double _BaseIndicator;
        private double _BaseScore;
        private double _Scale;
        private double _DecreaseStepScore;
        private double _IncreaseStepScore;

        public double BaseIndicator
        {
            get { return _BaseIndicator; }
            set { this.RaiseAndSetIfChanged(x => x.BaseIndicator, value); }
        }

        public double BaseScore
        {
            get { return _BaseScore; }
            set { this.RaiseAndSetIfChanged(x => x.BaseScore, value); }
        }

        public double Scale { get { return _Scale; } set { this.RaiseAndSetIfChanged(x => x.Scale, value); } }

        public double IncreaseStepScore
        {
            get { return _IncreaseStepScore; }
            set { this.RaiseAndSetIfChanged(x => x.IncreaseStepScore, value); }
        }

        public double DecreaseStepScore
        {
            get { return _DecreaseStepScore; }
            set { this.RaiseAndSetIfChanged(x => x.DecreaseStepScore, value); }
        }

        public string Type { get; set; }
    }
}