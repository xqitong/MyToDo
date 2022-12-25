﻿using MyToDo.Common;
using MyToDo.Extensions;
using MyToDo.Views;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyTodo.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        private readonly IDialogHostService dialogHostService;

        public MainView(IEventAggregator aggregator, IDialogHostService dialogHostService )
        {
            InitializeComponent();
            //注册等待消息窗口
            aggregator.Register(arg => {
                DialogHost.IsOpen = arg.IsOpen;
                if (DialogHost.IsOpen)
                {
                    DialogHost.DialogContent = new ProgressView();
                }

            });
            btnMin.Click += (s, e) =>
            {
                this.WindowState = WindowState.Minimized;
            };
            btnMax.Click += (s, e) => {

                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                }
            };
            btnClose.Click +=  async (s, e) => 
            {
                var dialogResult = await dialogHostService.Question("温馨提示","确认推出系统？");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK)
                {
                    return;
                }
                this.Close();
                
            };

            this.ColorZone.MouseDoubleClick += (s, e) => 
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                }
            };
            this.ColorZone.MouseMove += (s, e) => 
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();

                }
            };
            this.menuBar.SelectionChanged += (s, e) => 
            {
                this.drawerHost.IsLeftDrawerOpen = false;
            };
            this.dialogHostService = dialogHostService;
        }
    }
}
