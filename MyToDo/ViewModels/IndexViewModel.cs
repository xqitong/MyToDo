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
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            ToDoDtos =new ObservableCollection<ToDoDto>();
            MemoDtos =new ObservableCollection<MemoDto>();
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
                var removedModel = ToDoDtos.FirstOrDefault(t => t.Id.Equals(completedModel.Id));
                if (removedModel != null)
                {
                    ToDoDtos.Remove(removedModel);
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
                        var updateModel =  ToDoDtos.FirstOrDefault(t=> t.Id.Equals(newModel.Id));
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
                        ToDoDtos.Add(addResult.Result);
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
                        var updateModel = MemoDtos.FirstOrDefault(t => t.Id.Equals(newModel.Id));
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
                       MemoDtos.Add(addResult.Result);
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
        private ObservableCollection<ToDoDto> toDoDtos;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }

        }
        private ObservableCollection<MemoDto> memoDto;


        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDto; }
            set { memoDto = value; RaisePropertyChanged(); }

        }
        void CreateTaskBars()
        {
            TaskBars = new ObservableCollection<TaskBar>();
            TaskBars.Add(new TaskBar { Icon = "ClockFast", Title = "汇总", Color = "#ff0ca0ff", Content = "9", Target = "" });
            TaskBars.Add(new TaskBar { Icon = "ClockCheckOutline", Title = "已完成", Color = "#ff1eca3a", Content = "9", Target = "" });
            TaskBars.Add(new TaskBar { Icon = "ChartLineVariant", Title = "完成比例", Color = "#ff02c6dc", Content = "100%", Target = "" });
            TaskBars.Add(new TaskBar { Icon = "PlaylistStar", Title = "备忘录", Color = "#ffffa000", Content = "19", Target = "" });

        }

    }
}
