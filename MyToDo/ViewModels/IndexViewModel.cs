using ImTools;
using MyTodo.Common.Models;
using MyToDo.Common;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.ViewModels;
using MyToDo.Views.Dialogs;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodo.ViewModels
{
    public class IndexViewModel : NavigationViewModel
    {
        private readonly IToDoService toDoService;
        private readonly IMemoService memoService;
        private readonly IContainerProvider containerProvider;
        private readonly IDialogHostService dialog;
        public IndexViewModel(IContainerProvider containerProvider, IDialogHostService  dialog):base(containerProvider)
        {
            CreateTaskBars();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.containerProvider = containerProvider;
            toDoService = containerProvider.Resolve<IToDoService>();
            memoService = containerProvider.Resolve<IMemoService>();
            this.dialog = dialog;
            EditToDoCommand = new DelegateCommand<ToDoDto>(AddToDo);
            EditMemoCommand = new DelegateCommand<MemoDto>(AddMemo);
            ToDoCompletedCommand = new DelegateCommand<ToDoDto>(ToDoCompleted);


        }

        private async void ToDoCompleted(ToDoDto completedModel)
        {
            var updateResult = await toDoService.UpdateAsync(completedModel);
            if (updateResult.Status)
            {
                var removedModel = Summary.ToDoList.FirstOrDefault(t => t.Id.Equals(completedModel.Id));
                if (removedModel != null)
                {
                    Summary.ToDoList.Remove(removedModel);
                }
            }
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增待办": AddToDo(null);
                    break;
                case "新增备忘录":
                    AddMemo(null);
                    break;
                default:
                    break;
            }
        }

        private async void AddToDo(ToDoDto selectedModel)
        {
            DialogParameters param = new DialogParameters();
            if (selectedModel != null)
            {
                param.Add("Value", selectedModel);
            }
            var dialogResult = await dialog.ShowDialog("AddToDoView", param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                 var newModel = dialogResult.Parameters.GetValue<ToDoDto>("Value");
                if (newModel.Id > 0)
                {
                    var updateResult = await toDoService.UpdateAsync(newModel);
                    if (updateResult.Status)
                    {
                        var updateModel = Summary.ToDoList.FirstOrDefault(t=> t.Id.Equals(newModel.Id));
                        if (updateModel != null)
                        {
                            updateModel.Title = newModel.Title;
                            updateModel.Content = newModel.Content;
                        }
                    }
                }
                else
                {
                    var addResult = await toDoService.AddAsync(newModel);
                    if (addResult.Status)
                    {
                        Summary.ToDoList.Add(addResult.Result);
                    }
                }
            }
        }

        private async void AddMemo(MemoDto selectedModel)
        {

            DialogParameters param = new DialogParameters();
            if (selectedModel != null)
            {
                param.Add("Value", selectedModel);
            }
            var dialogResult = await dialog.ShowDialog("AddMemoView", param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var newModel = dialogResult.Parameters.GetValue<MemoDto>("Value");
                if (newModel.Id > 0)
                {
                    var updateResult = await memoService.UpdateAsync(newModel);
                    if (updateResult.Status)
                    {
                        var updateModel = Summary.MemoList.FirstOrDefault(t => t.Id.Equals(newModel.Id));
                        if (updateModel != null)
                        {
                            updateModel.Title = newModel.Title;
                            updateModel.Content = newModel.Content;
                        }
                    }
                }
                else
                {
                    var addResult = await memoService.AddAsync(newModel);
                    if (addResult.Status)
                    {
                        Summary.MemoList.Add(addResult.Result);
                    }
                }
            }
        }

        
        public DelegateCommand<ToDoDto> ToDoCompletedCommand { get; private set; }
        public DelegateCommand<string> ExecuteCommand{ get; private set; }

        public DelegateCommand<ToDoDto> EditToDoCommand { get; private set; }
        public DelegateCommand<MemoDto> EditMemoCommand { get; private set; }

        private ObservableCollection<TaskBar> taskBars;

        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value; RaisePropertyChanged(); }

        }
        //private ObservableCollection<ToDoDto> toDoDtos;

        //public ObservableCollection<ToDoDto> ToDoDtos
        //{
        //    get { return toDoDtos; }
        //    set { toDoDtos = value; RaisePropertyChanged(); }

        //}
        //private ObservableCollection<MemoDto> memoDto;


        //public ObservableCollection<MemoDto> MemoDtos
        //{
        //    get { return memoDto; }
        //    set { memoDto = value; RaisePropertyChanged(); }

        //}

        private SummaryDto summary;


        public SummaryDto Summary
        {
            get { return summary; }
            set { summary = value; RaisePropertyChanged(); }

        }
        void CreateTaskBars()
        {
            TaskBars = new ObservableCollection<TaskBar>();
            TaskBars.Add(new TaskBar { Icon = "ClockFast", Title = "汇总", Color = "#ff0ca0ff", Target = "" });
            TaskBars.Add(new TaskBar { Icon = "ClockCheckOutline", Title = "已完成", Color = "#ff1eca3a", Target = "" });
            TaskBars.Add(new TaskBar { Icon = "ChartLineVariant", Title = "完成比例", Color = "#ff02c6dc",  Target = "" });
            TaskBars.Add(new TaskBar { Icon = "PlaylistStar", Title = "备忘录", Color = "#ffffa000", Target = "" });

        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var summaryResult = await toDoService.SummaryAsync();
            if (summaryResult.Status)
            {
                Summary = summaryResult.Result;
                Refresh();
            }
            base.OnNavigatedTo(navigationContext);
        }
        void Refresh()
        {
            TaskBars[0].Content = Summary.Sum.ToString();
            TaskBars[1].Content = Summary.CompletedCount.ToString();
            TaskBars[2].Content = Summary.CompletedRatio;
            TaskBars[3].Content = Summary.MemoCount.ToString();

        }

    }
}
