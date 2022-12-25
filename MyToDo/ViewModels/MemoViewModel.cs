
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using MyToDo.ViewModels;
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
using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Extensions;

namespace MyToDo.ViewModels
{
    class MemoViewModel : NavigationViewModel
    {
        public MemoViewModel(IMemoService service, IContainerProvider containerProvider) : base(containerProvider)
        {
            this.service = service;
            MemoDtos = new ObservableCollection<MemoDto>();
            dialogHost = containerProvider.Resolve<IDialogHostService>();
            ExcuteCommand = new DelegateCommand<string>(Execute);
            SelectedCommand = new DelegateCommand<MemoDto>(Selected);
            DeleteCommand = new DelegateCommand<MemoDto>(Delete);


        }

        private async void Delete(MemoDto obj)
        {
            try
            {
                var dialogResult = await dialogHost.Question("温馨提示", $"确认删除备忘录事项:{obj.Title}?");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK)
                {
                    return;
                }
                UpdateLoading(true);
                var deleteResult = await service.DeleteAsync(obj.Id);
                if (deleteResult.Status)
                {
                    var model = MemoDtos.FirstOrDefault(t => t.Id.Equals(obj.Id));
                    if (model != null)
                    {
                        MemoDtos.Remove(model);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                UpdateLoading(false);
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
                        var Memo = MemoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                        if (Memo != null)
                        {
                            Memo.Title = CurrentDto.Title;
                            Memo.Content = CurrentDto.Content;
                            IsRightDrawerOpen = false;
                        }
                    }
                }
                else
                {
                    var addResult = await service.AddAsync(currentDto);
                    if (addResult.Status)
                    {
                        MemoDtos.Add(addResult.Result);
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




        private void Add()
        {
            IsRightDrawerOpen = true;
            CurrentDto = new MemoDto();
        }
        private async void Selected(MemoDto obj)
        {
            try
            {
                UpdateLoading(true);
                var MemoResult = await service.GetFirstOfDefaultAsync(obj.Id);
                if (MemoResult.Status)
                {
                    CurrentDto = MemoResult.Result;
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


            var MemoResult = await service.GetAllAsync(new QueryParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search,
            });
            if (MemoResult.Status)
            {
                MemoDtos.Clear();
                foreach (var item in MemoResult.Result.Items)
                {
                    MemoDtos.Add(item);
                }
            }
            UpdateLoading(false);
        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            GetDataAsync();
        }
        private ObservableCollection<MemoDto> memoDtos;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }

        private readonly IDialogHostService dialogHost;
        private bool isRightDrawerOpen;
        private readonly IMemoService service;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        private MemoDto currentDto;

        public MemoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<string> ExcuteCommand { get; private set; }
        public DelegateCommand<MemoDto> SelectedCommand { get; private set; }
        public DelegateCommand<MemoDto> DeleteCommand { get; private set; }
    }
}
