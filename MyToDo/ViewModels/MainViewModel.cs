using MyTodo.Common.Models;
using MyTodo.Extensions;
using MyToDo.Common;
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
    public class MainViewModel : BindableBase, IConfigureService
    {
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }


        public MainViewModel(IContainerProvider containerProvider, IRegionManager regionManager)
        {

            this.containerProvider = containerProvider;
            MenuBars = new ObservableCollection<MenuBar>();

            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            LogoutCommand = new DelegateCommand(()=>{
                App.Logout(this.containerProvider);

            });
            this.regionManager = regionManager;
            GoBackCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoBack)
                {
                    journal.GoBack();
                }
            });
            GoForwardCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoForward)
                {
                    journal.GoForward();
                }
            });
        }

        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.NameSpace))
            {
                return;
            }
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });


        }

        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }

        public DelegateCommand LogoutCommand{ get; private set; }

        private ObservableCollection<MenuBar> menuBars;
        private readonly IContainerProvider containerProvider;
        private readonly IRegionManager regionManager;
        private IRegionNavigationJournal journal;
        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }

        void CreateMenuBars()
        {
            MenuBars.Add(new MenuBar { Icon = "Home", Title = "首页", NameSpace = "IndexView" });
            MenuBars.Add(new MenuBar { Icon = "NotebookOutline", Title = "待办事项", NameSpace = "ToDoView" });
            MenuBars.Add(new MenuBar { Icon = "NotebookPlus", Title = "备忘录", NameSpace = "MemoView" });
            MenuBars.Add(new MenuBar { Icon = "Cog", Title = "设置", NameSpace = "SettingsView" });

        }

        public void Configure()
        {
            UserName = AppSession.UserName;
            CreateMenuBars();
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView");
        }
    }
}
