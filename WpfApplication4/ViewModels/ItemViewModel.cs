﻿using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;
using Telerik.Windows.Controls;
using WpfApplication4.Model;

namespace WpfApplication4.ViewModels
{
    public class ItemViewModel : ReactiveObject
    {
        private bool _IsEditing;
        private IEvaluationItemDescription _Description;

        public ItemViewModel()
        {
            Operations = new ObservableCollection<HyperCommand>();
        }

        public string Id { get; protected set; }

        public virtual string Name { get; set; }

        public string StatisticalWay { get; set; }

        public IFormulaParams FormulaParams { get; set; }


        public bool IsEditing
        {
            get { return _IsEditing; }
            set
            {
                this.RaiseAndSetIfChanged(x => x.IsEditing, value);
            }
        }

        public IEvaluationItemDescription Description
        {
            get { return _Description; }
            set { this.RaiseAndSetIfChanged(x => x.Description, value); }
        }

        public ObservableCollection<HyperCommand> Operations { get; set; }

        public static ItemViewModel Create(EvaluationItem model)
        {
            if (model == null)
                return new UndefinedViewModel();
            if (string.IsNullOrEmpty(model.Id))
                return new NewItemViewModel();
            if (model.Status == "deleted")
            {
                return new DeletedViewModel { Id = model.Id, Name = model.Name, StatisticalWay = model.StatisticalWay };
            }

            if (model.Links != null && model.Links.Any(o => o.Method == "PATCH"))
            {
                var vm = new ItemEditViewModel(model);

                return vm;
            }

            return new ItemViewModel
            {
                Id = model.Id,
                Name = model.Name,
                StatisticalWay = model.StatisticalWay,
                FormulaParams = model.FormulaParams.ConvertTo<IFormulaParams>()
            };
        }

        public virtual void OperationAdded()
        {
        }
    }
}