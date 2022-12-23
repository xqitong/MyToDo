using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
    public class BaseDto : INotifyPropertyChanged
    {
        private int id;
        private DateTime createTime;
        private DateTime updateTime;

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        public DateTime CreateTime
        {
            get => createTime; 
            set
            {
                createTime = value;
                OnPropertyChanged();
            }
        }
        public DateTime UpdateTime
        {
            get => updateTime; 
            set
            {
                updateTime = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
