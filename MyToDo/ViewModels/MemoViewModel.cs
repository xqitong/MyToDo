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
    class MemoViewModel : BindableBase
    {
        public MemoViewModel(IMemoService service)
        {
            MemoDtos = new ObservableCollection<MemoDto>();
            
            AddCommand = new DelegateCommand(AddCommandFunc);
            this.service = service;
            CreateToDoDtos();
        }

        private void AddCommandFunc()
        {
            IsRightDrawerOpen = true;
        }

        async private void CreateToDoDtos()
        {
            var memoResult = await service.GetAllAsync(new QueryParameter()
            {
                PageIndex = 0,
                PageSize = 100,
            });
            if (memoResult.Status)
            {
                MemoDtos.Clear();
                foreach (var item in memoResult.Result.Items)
                {
                    MemoDtos.Add(item);
                }
            }
        }

        private ObservableCollection<MemoDto> memoDtos;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }
        private bool isRightDrawerOpen;
        private readonly IMemoService service;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        public DelegateCommand AddCommand { get; private set; }
    }
}
