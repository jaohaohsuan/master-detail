using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using Grandsys.Wfm.Services.Outsource.ServiceModel;
using ReactiveUI;
using ServiceStack.Common;
using ServiceStack.Text;
using WpfApplication4.Model;

namespace WpfApplication4.ViewModels
{
    public class ItemEditViewModel : ItemViewModel
    {
        private readonly IFormulaParams _SelectedFormula;
        private readonly UpateRequestBody _updateValues;
        private string _name;

        public ItemEditViewModel(EvaluationItem model)
        {
            Id = model.Id;
            _name = model.Name;
            StatisticalWay = model.StatisticalWay;
            Description = model.Description.ConvertTo<IEvaluationItemDescription>();
            var obsvr = Observer.Create<FormulaInfo>(o =>
            {
                if (_updateValues == null)
                    return;
                _updateValues.Formula = o;
            });

            Observable.FromEventPattern<PropertyChangedEventArgs>(Description, "PropertyChanged").Select(args => args.Sender).OfType<IEvaluationItemDescription>().Subscribe(o =>
            {
                if (_updateValues == null)
                    return;
                _updateValues.Description = o;
            });

           SetFormulaOptions  = model.FormulaOptions.Select(o =>
            {
                var vm = o.ConvertTo<IFormulaParams>();
                if (vm != null)
                    Observable.FromEventPattern<PropertyChangedEventArgs>(vm, "PropertyChanged")
                        .Select(args => args.Sender)
                        .OfType<IFormulaParams>().Select(p => new FormulaInfo().PopulateWith(p))
                        .Subscribe(obsvr);

                return vm;
            }).ToList();

            FormulaParams = model.FormulaParams.ConvertTo<IFormulaParams>();

            if (FormulaParams != null)
            {
                _SelectedFormula = SetFormulaOptions.FirstOrDefault(o => FormulaParams != null && FormulaParams.GetType() == o.GetType());
            }

            this.WhenAny(x => x.SelectedFormula, x => x.Value)
                .Select(o => o == null ? null : new FormulaInfo().PopulateWith(o))
                .Subscribe(obsvr);
            
            IsEditing = true;
            _updateValues = new UpateRequestBody();
        }

        public IEnumerable<IFormulaParams> SetFormulaOptions { get; set; }

        public UpateRequestBody UpdateValues { get { return _updateValues; } }

        public override string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                WriteToRequest(updateValues => { updateValues.Name = value; });
            }
        }

        public IFormulaParams SelectedFormula
        {
            get { return _SelectedFormula; }
            set { this.RaiseAndSetIfChanged(x => x.SelectedFormula, value); }
        }

        private void WriteToRequest(Action<UpateRequestBody> set)
        {
            if (UpdateValues != null)
                set(UpdateValues);
        }
    }
}