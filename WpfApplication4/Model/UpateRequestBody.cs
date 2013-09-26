using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grandsys.Wfm.Services.Outsource.ServiceModel;

namespace WpfApplication4.Model
{
    public class UpateRequestBody
    {
        public FormulaInfo Formula { get; set; }

        public IEvaluationItemDescription Description { get; set; }
        
        public string Name { get; set; }
    }
}
