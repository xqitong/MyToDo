using MyTodo.Common.Models;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodo.ViewModels
{
    public class ToDoViewModel:BindableBase
    {
		public ToDoViewModel(IToDoService service)
		{
			this.service = service;
			ToDoDtos = new ObservableCollection<ToDoDto>();
			
			AddCommand = new DelegateCommand(AddCommandFunc);
            
            CreateToDoDtos();
        }

        private void AddCommandFunc()
        {
			IsRightDrawerOpen = true;
        }

        async private void CreateToDoDtos()
        {
			var todoResult  = await service.GetAllAsync(new QueryParameter() {
				PageIndex= 0,
				PageSize= 100,
			});
			if (todoResult.Status)
			{
				ToDoDtos.Clear();
				foreach (var item in todoResult.Result.Items)
				{
					ToDoDtos.Add(item);
				}
			}
        }

        private ObservableCollection<ToDoDto> toDoDtos;

		public ObservableCollection<ToDoDto> ToDoDtos
        {
			get { return toDoDtos; }
			set { toDoDtos = value; RaisePropertyChanged(); }
		}
		private bool isRightDrawerOpen;
        private readonly IToDoService service;

        public bool IsRightDrawerOpen
        {
			get { return isRightDrawerOpen; }
			set { isRightDrawerOpen = value; RaisePropertyChanged(); }
		}

		public DelegateCommand AddCommand{ get; private set; }
	}
}
