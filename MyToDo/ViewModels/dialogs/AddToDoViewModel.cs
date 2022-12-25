﻿using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels.dialogs
{
    public class AddToDoViewModel : IDialogHostAware
    {
        public AddToDoViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);

        }

        private void Save()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogParameters param = new DialogParameters();
                DialogHost.Close(DialogHostName,new DialogResult(ButtonResult.OK, param));
            }
        }

        private void Cancel()
        {
            DialogParameters param = new DialogParameters();
            DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No, param));
        }

        public string DialogHostName { get ; set ; }
        public DelegateCommand SaveCommand { get;  set; }
        public DelegateCommand CancelCommand { get;  set; }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}
