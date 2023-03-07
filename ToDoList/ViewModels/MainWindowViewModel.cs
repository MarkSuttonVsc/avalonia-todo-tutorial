using ReactiveUI;
using System;
using System.Reactive.Linq;
using ToDoList.DataModel;
using ToDoList.Services;

namespace ToDoList.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _contentViewModel;
        public ToDoListViewModel ToDoListViewModel { get; }

        //this has a dependency on the ToDoListService

        public MainWindowViewModel()
        {
            var service = new ToDoListService();
            ToDoListViewModel = new ToDoListViewModel(service.GetItems());
            _contentViewModel = ToDoListViewModel;
        }

        public ViewModelBase ContentViewModel
        {
            get => _contentViewModel;
            private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
        }

        public void AddItem()
        {
            AddItemViewModel addItemViewModel = new();

            Observable.Merge(
                addItemViewModel.OkCommand,
                addItemViewModel.CancelCommand.Select(_ => (ToDoItem?)null))
                .Take(1)
                .Subscribe(model =>
                {
                    if (model != null)
                    {
                        ToDoListViewModel.ListItems.Add(model);
                    }

                    ContentViewModel = ToDoListViewModel;
                });

            ContentViewModel = addItemViewModel;
        }
    }
}


