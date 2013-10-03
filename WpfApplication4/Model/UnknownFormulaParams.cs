namespace WpfApplication4.Model
{
    public class UnknownFormulaParams : IFormulaParams
    {
        public UnknownFormulaParams()
        {
            Type = "unknown";
        }

        public string Type { get; private set; }
    }
}