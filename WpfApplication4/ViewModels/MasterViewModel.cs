using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Threading;
using Grandsys.Wfm.Services.Outsource.ServiceModel;
using ReactiveUI;
using ReactiveUI.Xaml;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;
using WpfApplication4.Model;
using GetEvaluationItem = WpfApplication4.Model.GetEvaluationItem;

namespace WpfApplication4.ViewModels
{
    public partial class MasterViewModel
    {
        private class EvaluationItems : IReturn<List<EvaluationItemTitle>>
        {
        }
    }

    public partial class MasterViewModel : ReactiveObject
    {
        private readonly JsonServiceClient _client;
        private ObservableAsPropertyHelper<ItemViewModel> _CurrentViewModel;
        private ObservableAsPropertyHelper<ReactiveCollection<EvaluationItemTitle>> _Items;
        private EvaluationItemTitle _selectedItem;

        public MasterViewModel()
        {
            var viewModelSource = new BehaviorSubject<ItemViewModel>(new UndefinedViewModel());
            var hyperVm = new Subject<Tuple<IEnumerable<Link>, ItemViewModel>>();

            viewModelSource.ToProperty(this, x => x.CurrentViewModel);

            _client = new JsonServiceClient("http://grandsysapi.azurewebsites.net/");

#if DEBUG
            //_client = new JsonServiceClient("http://localhost:35138/");
#endif

            GetAll = new ReactiveAsyncCommand();
            New = new ReactiveAsyncCommand();

            var selectedItemChanged = this.WhenAny(x => x.SelectedItem, x => x.Value);

            var getEvaluationItem = selectedItemChanged.Throttle(
                TimeSpan.FromMilliseconds(300)).ObserveOn(Scheduler.Default)
                .Where(x => x != null && x.Id != CurrentViewModel.Id)
                .Select(o => _client.Get(new GetEvaluationItem(o.Id)));

            New.RegisterAsyncFunction(_ =>
            {
                var response = _client.Get(new EvaluationItemsCreationWays());
                IEnumerable<Link> links = new List<Link>(response.Links) { new Link { Name = "Discard" } };
                ItemViewModel vm = new NewItemViewModel();
                return Tuple.Create(links, vm);
            }).Subscribe(hyperVm);

            GetAll.RegisterAsyncFunction(_ => _client.Get(new EvaluationItems()).ToObservable().CreateCollection()).ToProperty(this, x => x.Items);

            this.WhenAny(x => x.Items, x => x.Value)
                .Where(o => o != null)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Subscribe(o => { SelectedItem = o.FirstOrDefault(a => Equals(a.Id, CurrentViewModel.Id)); });

            var responseObsvr = Observer.Create<EvaluationItem>(o =>
            {
                if (o == null)
                {
                    viewModelSource.OnNext(new UndefinedViewModel());
                    return;
                }
                hyperVm.OnNext(Tuple.Create(o.Links, ItemViewModel.Create(o)));
            });

            hyperVm.Subscribe(tuple =>
            {
                var links = tuple.Item1;
                var vm = tuple.Item2;
                foreach (var link in links)
                {
                    var cmd = new HyperCommand(_client, link);
                    if (link.Method != "GET")
                    {
                        cmd.Response.Throttle(TimeSpan.FromMilliseconds(500)).Subscribe(_ => { GetAll.Execute(null); });
                    }
                    cmd.Response.Subscribe(responseObsvr);
                    vm.Operations.Add(cmd);
                }
                viewModelSource.OnNext(vm);
            });

            getEvaluationItem.Subscribe(responseObsvr);
        }

        public ItemViewModel CurrentViewModel { get { return _CurrentViewModel.Value; } }

        public ReactiveAsyncCommand GetAll { get; set; }
        public ReactiveAsyncCommand New { get; set; }

        public ReactiveCollection<EvaluationItemTitle> Items { get { return _Items.Value; } }

        public EvaluationItemTitle SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (!CurrentViewModel.IsEditing)
                {
                    _selectedItem = value;
                }

                Application.Current.Dispatcher.BeginInvoke(
                    new Action(() => { this.RaisePropertyChanged(x => x.SelectedItem); }),
                    DispatcherPriority.ContextIdle);
            }
        }
    }
}