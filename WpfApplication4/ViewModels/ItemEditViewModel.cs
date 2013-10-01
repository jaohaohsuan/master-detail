using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Grandsys.Wfm.Services.Outsource.ServiceModel;
using ReactiveUI;
using ServiceStack.Common;
using WpfApplication4.Model;

namespace WpfApplication4.ViewModels
{
    public class ItemEditViewModel : ItemViewModel
    {
        private readonly string _Name;
        private readonly IFormulaParams _SelectedFormula;
        private readonly UpateRequestBody _updateValues;

        public ItemEditViewModel(EvaluationItem model)
        {
            Id = model.Id;
            _Name = model.Name;
            StatisticalWay = model.StatisticalWay;
            FormulaParams = model.FormulaParams.ConvertTo<IFormulaParams>();
            Description = model.Description.ConvertTo<IEvaluationItemDescription>();

            var obsvr = Observer.Create<FormulaInfo>(o =>
            {
                if (_updateValues == null)
                    return;
                _updateValues.Formula = o;
            });

            Observable.FromEventPattern<PropertyChangedEventArgs>(Description, "PropertyChanged")
                .Select(args => args.Sender)
                .OfType<IEvaluationItemDescription>()
                .Subscribe(o =>
                {
                    if (_updateValues == null)
                        return;
                    _updateValues.Description = o;
                });

            SetFormulaOptions = model.FormulaOptions.Select(o =>
            {
                var vm = o.ConvertTo<IFormulaParams>();
                if (vm != null)
                    Observable.FromEventPattern<PropertyChangedEventArgs>(vm, "PropertyChanged")
                        .Select(args => args.Sender)
                        .OfType<IFormulaParams>().Select(p => new FormulaInfo().PopulateWith(p))
                        .Subscribe(obsvr);

                return vm ?? new UnknownFormulaParams();
            }).ToList();

            _SelectedFormula = SetFormulaOptions.FirstOrDefault(o => FormulaParams != null && FormulaParams.GetType() == o.GetType());


            this.WhenAny(x => x.SelectedFormula, x => x.Value)
                .Select(o => o == null ? null : new FormulaInfo().PopulateWith(o))
                .Subscribe(obsvr);


            this.WhenAny(x => x.Name, x => x.Value)
                .Subscribe(o =>
                {
                    if (_updateValues == null)
                        return;
                    _updateValues.Name = o;
                });

            IsEditing = true;
            _updateValues = new UpateRequestBody();
        }

        public IEnumerable<IFormulaParams> SetFormulaOptions { get; set; }

        public UpateRequestBody UpdateValues { get { return _updateValues; } }

        public override string Name { get { return _Name; } set { this.RaiseAndSetIfChanged(x => x.Name, value); } }

        public IFormulaParams SelectedFormula
        {
            get { return _SelectedFormula; }
            set { this.RaiseAndSetIfChanged(x => x.SelectedFormula, value); }
        }
    }
}