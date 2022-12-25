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
            TaskBars = new ObservableCollection<TaskBar>();
            ToDoDtos =new ObservableCollection<ToDoDto>();
            MemoDtos =new ObservableCollection<MemoDto>();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.containerProvider = containerProvider;
            toDoService = containerProvider.Resolve<IToDoService>();
            memoService = containerProvider.Resolve<IMemoService>();
            this.dialog = dialog;
            CreateTaskBars();
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增待办": AddToDo();
                    break;
                case "新增备忘录":
                    AddMemo();
                    break;
                default:
                    break;
            }
        }

        private async void AddToDo()
        {
            var dialogResult = await dialog.ShowDialog("AddToDoView",null);
            if (dialogResult.Result == ButtonResult.OK)
            {
                 var model = dialogResult.Parameters.GetValue<ToDoDto>("Value");
                if (model.Id > 0)
                {
                    
                }
                else
                {
                    var addResult = await toDoService.AddAsync(model);
                    if (addResult.Status)
                    {
                        ToDoDtos.Add(addResult.Result);
                    }
                }
            }
        }

        private async void AddMemo()
        {

            var dialogResult = await dialog.ShowDialog("AddMemoView", null);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var model = dialogResult.Parameters.GetValue<MemoDto>("Value");
                if (model.Id > 0)
                {

                }
                else
                {
                    var addResult = await memoService.AddAsync(model);
                    if (addResult.Status)
                    {
                       MemoDtos.Add(addResult.Result);
                    }
                }
            }
        }

        public DelegateCommand<string> ExecuteCommand{ get; private set; }


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
            TaskBars.Add(new TaskBar { Icon = "ClockFast", Title = "汇总", Color = "#ff0ca0ff", Content = "9", Target = "" });
            TaskBars.Add(new TaskBar { Icon = "ClockCheckOutline", Title = "已完成", Color = "#ff1eca3a", Content = "9", Target = "" });
            TaskBars.Add(new TaskBar { Icon = "ChartLineVariant", Title = "完成比例", Color = "#ff02c6dc", Content = "100%", Target = "" });
            TaskBars.Add(new TaskBar { Icon = "PlaylistStar", Title = "备忘录", Color = "#ffffa000", Content = "19", Target = "" });

        }

    }
}
