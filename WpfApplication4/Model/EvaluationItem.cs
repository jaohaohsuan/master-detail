using System.Collections.Generic;
using Grandsys.Wfm.Services.Outsource.ServiceModel;

namespace WpfApplication4.Model
{
    public class EvaluationItem
    {
        public EvaluationItem()
        {
            Links = new List<Link>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string StatisticalWay { get; set; }

        public Dictionary<string, string> FormulaParams { get; set; }

        public Dictionary<string, string> Description { get; set; }

        public IEnumerable<Link> Links { get; set; }

        public IEnumerable<Dictionary<string, string>> FormulaOptions { get; set; }
    }
}