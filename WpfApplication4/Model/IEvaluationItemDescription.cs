using System.ComponentModel;
using ReactiveUI;

namespace WpfApplication4.Model
{
    public interface IEvaluationItemDescription : INotifyPropertyChanged
    {
    }

    public interface IFormulaParams
    {
    }

    public class LinearFormulaParams : IFormulaParams
    {
        public double BaseIndicator { get; set; }
        public double BaseScore { get; set; }
        public double Scale { get; set; }
        //linear
        public double IncreaseStepScore { get; set; }
        public double DecreaseStepScore { get; set; }


    }

    public class SlideFormulaParams : IFormulaParams
    {
        public double BaseIndicator { get; set; }
        public double BaseScore { get; set; }
        public double Scale { get; set; }
        //slide
        public double StepScore { get; set; }
        public double StartIndicator { get; set; }
        public double FinalIndicator { get; set; }
    }
}