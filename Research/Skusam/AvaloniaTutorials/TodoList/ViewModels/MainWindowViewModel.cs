﻿using System;
using System.Reactive.Linq;
using ReactiveUI;
using ToDoList.DataModel;
using ToDoList.Services;

namespace ToDoList.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _contentViewModel;
    public MainWindowViewModel()
    {
        var service = new ToDoListService();
        ToDoList = new ToDoListViewModel(service.GetItems());
        _contentViewModel = ToDoList;
    }
    public ToDoListViewModel ToDoList { get; }

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
            .Subscribe(newItem =>
            {
                if (newItem != null)
                {
                    ToDoList.ListItems.Add(newItem);
                }

                ContentViewModel = ToDoList;
            });
        ContentViewModel = addItemViewModel;
    }
}
