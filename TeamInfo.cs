using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public class TeamInfo : INotifyPropertyChanged
    {
        private string _TeamName;//название команды
        public string TeamName
        {
            get => _TeamName;
            set
            {
                _TeamName = value;
                OnPropertyChanged("TeamName");//или OnPropertyChanged();
            }
        }
        private string _TeamTitle;//обозначение команды хозяева/готсти/друзья/племянники и т.д.
        public string TeamTitle
        {
            get => _TeamTitle;
            set
            {
                _TeamTitle = value;
                OnPropertyChanged("TeamTitle");//или OnPropertyChanged();
            }
        }
        private short _TeamCounter = 0;//счетчик очков команды
        public short TeamCounter 
        { 
            get => _TeamCounter; 
            set
            {
                _TeamCounter = value;
                OnPropertyChanged("TeamCounter");//или OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")//цель [CallerMemberName] ставить значение вызываемого set-era умолчанию, если его вызвали с пустой строкой, т.е. OnPropertyChanged()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


    }
}
