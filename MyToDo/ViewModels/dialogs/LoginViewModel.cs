using MyToDo.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels.dialogs
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        public LoginViewModel(ILoginService loginService)
        {
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.loginService = loginService;
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "Login": Login(); break;
                case "LogOut": LogOut(); break;
                default:
                    break;
            }
        }

        private void LogOut()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        private async void Login()
        {
            if (string.IsNullOrWhiteSpace(Account) || string.IsNullOrWhiteSpace(PassWord))
            {
                return;
            }
            var loginResult = await loginService.LoginAsync(new Shared.Dtos.UserDto()
            { 
                Account = Account,
                Password = PassWord
            });
            if (loginResult.Status)
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            // login failed

        }

        public string Title { get; set; } = "ToDo";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            LogOut();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }

        public DelegateCommand<string> ExecuteCommand;

        private string account;

        public string Account
        {
            get { return account; }
            set { account = value; RaisePropertyChanged(); }
        }

        private string passWord;
        private readonly ILoginService loginService;

        public string  PassWord
        {
            get { return passWord; }
            set { passWord = value; RaisePropertyChanged(); }
        }


    }
}
