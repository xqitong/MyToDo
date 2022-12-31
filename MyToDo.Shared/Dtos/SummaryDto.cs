﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
    public class SummaryDto : BaseDto
    {
        public int Sum
        {
            get => sum;
            set
            {
                sum = value; OnPropertyChanged();
            }
        }
        public int CompletedCount
        {
            get => completedCount;
            set
            {
                completedCount = value; OnPropertyChanged();
            }
        }
        public int MemoCount
        {
            get => memoCount;
            set
            {
                memoCount = value; OnPropertyChanged();
            }
        }
        public string CompletedRatio
        {
            get => completedRatio; 
            set
            {
                completedRatio = value; OnPropertyChanged();
            }
        }

        private ObservableCollection<ToDoDto> toDoList;
        private ObservableCollection<MemoDto> memoList;
        private int sum;
        private int completedCount;
        private int memoCount;
        private string completedRatio;

        public ObservableCollection<ToDoDto> ToDoList
        {
            get { return toDoList; }
            set { toDoList = value; OnPropertyChanged(); }
        }

        public ObservableCollection<MemoDto> MemoList
        {
            get { return memoList; }
            set { memoList = value; OnPropertyChanged(); }
        }

    }
}
