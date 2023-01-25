﻿using MyToDo.Common;
using MyToDo.Common.Events;
using Newtonsoft.Json.Bson;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Extensions
{
    public static class DialogExtension
    {
        public static void UpdateLoading(this IEventAggregator aggregator,UpdateModel model) 
        {
                aggregator.GetEvent<UpdateLoadingEvent>().Publish(model);
        }
        public static void Register(this IEventAggregator aggregator,Action< UpdateModel> action)
        {
            aggregator.GetEvent<UpdateLoadingEvent>().Subscribe(action );
        }
        public static async Task<IDialogResult> Question(this IDialogHostService dialogHost
        ,string title,string content,string dialogHostName="Root")
        {
            DialogParameters param = new DialogParameters(); 
            param.Add("Title", title);
            param.Add("Content", content);
            var dialogResult = await dialogHost.ShowDialog("MsgView", param, dialogHostName);
            return dialogResult;
        }

        public static void RegisterMessage(this IEventAggregator aggregator,
                    Action<MessageModel> action,string filterName="Main") 
        {
            aggregator.GetEvent<MessageEvent>().Subscribe(action,
            ThreadOption.PublisherThread,true,(m)=>{
                return m.Filter.Equals(filterName);
            });
        }
        public static void SendMessage(this IEventAggregator aggregator,string message,string filterName="Main") 
        {
            aggregator.GetEvent<MessageEvent>().Publish(new MessageModel()
            {
                Filter = filterName,
                Message = message
            }) ;
        }
    }
}
