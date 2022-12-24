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

            ExcuteCommand = new DelegateCommand<string>(Execute);
			SelectedCommand = new DelegateCommand<ToDoDto>(Selected);
            DeleteCommand = new DelegateCommand<ToDoDto>(Delete);


        }

        private async void Delete(ToDoDto obj)
        {
            var deleteResult = await service.DeleteAsync(obj.Id);
            if(deleteResult.Status)
            {
                var model = ToDoDtos.FirstOrDefault(t=> t.Id.Equals(obj.Id));
                if(model != null) 
                {
                    ToDoDtos.Remove(model);
                }
            }
        }

        private void Execute(string obj)
        {
			switch (obj)
			{
				case "新增": 
					Add();
					break;
				case "查询":
                    GetDataAsync();
					break;
				case "保存":
					 Save();
					break;
				default:
					break;
			}
		}

        private async void Save()
        {
			if (string.IsNullOrWhiteSpace(CurrentDto.Title) || string.IsNullOrWhiteSpace(CurrentDto.Content))
			{
				return;
			}
			UpdateLoading(true);
			try
			{
                if (currentDto.Id > 0)
                {
                    var updateResult = await service.UpdateAsync(CurrentDto);
                    if (updateResult.Status)
                    {
                        var todo = ToDoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                        if (todo != null)
                        {
                            todo.Title = CurrentDto.Title;
                            todo.Content = CurrentDto.Content;
                            todo.Status = CurrentDto.Status;
                            IsRightDrawerOpen = false;
                        }
                    }
                }
                else
                {
                    var addResult = await service.AddAsync(currentDto);
                    if (addResult.Status)
                    {
                        ToDoDtos.Add(addResult.Result);
                        IsRightDrawerOpen = false;
                    }
                }
            }
			catch (Exception)
			{

            }
             finally
             {
                UpdateLoading(false);
            }
           

        }

        private string search;

		public string Search
		{
			get { return search; }
			set { search = value; RaisePropertyChanged(); }
		}

        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; RaisePropertyChanged(); }
        }



        private void Add()
        {
			IsRightDrawerOpen = true;
            CurrentDto = new ToDoDto();
        }
        private async void Selected(ToDoDto obj)
        {
			try
			{
                UpdateLoading(true);
                var todoResult = await service.GetFirstOfDefaultAsync(obj.Id);
                if (todoResult.Status)
                {
                    CurrentDto = todoResult.Result;
                    IsRightDrawerOpen = true;
                }
                UpdateLoading(false);
            }
			catch (Exception)
			{

                UpdateLoading(false);
            }


        }
        async private void GetDataAsync()
        {
			UpdateLoading(true);

            int? status = selectedIndex == 0 ? null :( SelectedIndex == 2? 1: 0);
            var todoResult = await service.GetAllAsync(new ToDoParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search,
                Status = status
            }) ;
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

		private ToDoDto currentDto;

		public ToDoDto CurrentDto
        {
			get { return currentDto; }
			set { currentDto = value; RaisePropertyChanged(); }
		}

		public DelegateCommand<string> ExcuteCommand{ get; private set; }
		public DelegateCommand<ToDoDto> SelectedCommand{ get; private set; }
        public DelegateCommand<ToDoDto> DeleteCommand { get; private set; }
        

    }
}
