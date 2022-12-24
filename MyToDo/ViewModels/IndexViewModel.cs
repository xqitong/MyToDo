using MyTodo.Common.Models;
using MyToDo.Shared.Dtos;
using MyToDo.Views.Dialogs;
using Prism.Commands;
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
    public class IndexViewModel : BindableBase
    {
        public IndexViewModel(IDialogService  dialog)
        {
            TaskBars = new ObservableCollection<TaskBar>();
            ToDoDtos =new ObservableCollection<ToDoDto>();
            MemoDtos =new ObservableCollection<MemoDto>();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.dialog = dialog;
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

        private void AddToDo()
        {
            dialog.ShowDialog("AddToDoView");
        }

        private void AddMemo()
        {
            dialog.ShowDialog("AddMemoView");
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
        private readonly IDialogService dialog;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDto; }
            set { memoDto = value; RaisePropertyChanged(); }

        }

    }
}
