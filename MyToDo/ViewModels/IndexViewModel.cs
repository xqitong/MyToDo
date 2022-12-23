using MyTodo.Common.Models;
using MyToDo.Shared.Dtos;
using Prism.Mvvm;
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
        public IndexViewModel()
        {
            TaskBars = new ObservableCollection<TaskBar>();
            ToDoDtos = new ObservableCollection<ToDoDto>();
            MemoDtos= new ObservableCollection<MemoDto>();
            CreateTaskBars();
            CreateTestData();
        }

        private void CreateTestData()
        {
            for (int i = 0; i < 10; i++)
            {
                ToDoDtos.Add(new ToDoDto { Title = "ToDO" + i, Content = "in progress" });
                MemoDtos.Add(new MemoDto { Title = "Memmo" + i, Content = "my code" });

            }
        }

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
