using MyTodo.Common.Models;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using MyToDo.ViewModels;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodo.ViewModels
{
    public class ToDoViewModel: NavigationViewModel
    {
		public ToDoViewModel(IToDoService service, IContainerProvider containerProvider):base(containerProvider)
		{
			this.service = service;
			ToDoDtos = new ObservableCollection<ToDoDto>();
			
			AddCommand = new DelegateCommand(AddCommandFunc);
            
        }

        private void AddCommandFunc()
        {
			IsRightDrawerOpen = true;
        }

        async private void GetDataAsync()
        {
			UpdateLoading(true);
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
            UpdateLoading(false);
        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
			GetDataAsync();
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
