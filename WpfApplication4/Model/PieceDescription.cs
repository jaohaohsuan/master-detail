using ReactiveUI;

namespace WpfApplication4.Model
{
    public class PieceDescription : ReactiveObject, IEvaluationItemDescription
    {
        private string _Title;
        private string _Unit;
        public string Title { get { return _Title; } set { this.RaiseAndSetIfChanged(x => x.Title, value); } }

        public string Unit { get { return _Unit; } set { this.RaiseAndSetIfChanged(x => x.Unit, value); } }
    }
}