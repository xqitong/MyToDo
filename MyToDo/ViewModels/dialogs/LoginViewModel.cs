using DryIoc;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
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
            RegisterUserDto = new RegisterUserDto();
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "Login": 
                    Login(); 
                    break;
                case "LogOut": 
                    LogOut(); 
                    break;
                case "Go":
                    SelectedIndex = 1;
                    break;
                case "Return":
                    SelectedIndex = 0;
                    break;
                case "Register":
                    Register();
                    break;
                default:
                    break;
            }
        }

        private async void Register()
        {
            if (string.IsNullOrWhiteSpace(RegisterUserDto.Account) ||
            string.IsNullOrWhiteSpace(RegisterUserDto.Name) ||
            string.IsNullOrWhiteSpace(RegisterUserDto.Password) ||
            string.IsNullOrWhiteSpace(RegisterUserDto.NewPassword))
            {
                return;
            }
            if (RegisterUserDto.Password != RegisterUserDto.NewPassword)
            {
                return;
            }
            var registerResult = await loginService.RegisterAsync(new Shared.Dtos.UserDto()
            {
                Account = RegisterUserDto.Account,
                Name= RegisterUserDto.Name,
                Password= RegisterUserDto.Password,
            });
            if (registerResult!=null && registerResult.Status)
            {
               //register success 
               SelectedIndex = 0; 
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
                Name = "test", //不能为空，要不回请求失败
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

        public DelegateCommand<string> ExecuteCommand { get; set; }

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

        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; RaisePropertyChanged(); }
        }
        private RegisterUserDto registerUserDto;

        public RegisterUserDto RegisterUserDto
        {
            get { return registerUserDto; }
            set { registerUserDto = value; }
        }


    }
}
