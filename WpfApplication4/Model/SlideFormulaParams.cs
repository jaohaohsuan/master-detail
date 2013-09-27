using ReactiveUI;

namespace WpfApplication4.Model
{
    public class SlideFormulaParams : ReactiveObject, IFormulaParams
    {
        private double _BaseIndicator;
        private double _BaseScore;
        private double _Scale;
        private double _FinalIndicator;
        private double _StartIndicator;
        private double _StepScore;
        
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
        
        public double StepScore
        {
            get { return _StepScore; }
            set { this.RaiseAndSetIfChanged(x => x.StepScore, value); }
        }

        public double StartIndicator
        {
            get { return _StartIndicator; }
            set { this.RaiseAndSetIfChanged(x => x.StartIndicator, value); }
        }

        public double FinalIndicator
        {
            get { return _FinalIndicator; }
            set { this.RaiseAndSetIfChanged(x => x.FinalIndicator, value); }
        }

        public string Type { get { return "slide"; }}
    }
}