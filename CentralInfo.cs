using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public class CentralInfo : INotifyPropertyChanged
    {
        private string _TabloName;
        public string TabloName
        {
            get => _TabloName;
            set
            {
                _TabloName = value;
                OnPropertyChanged("TabloName");
            }
        }
        private short _Period;
        public short Period
        {
            get => _Period;
            set
            {
                _Period = value;
                OnPropertyChanged("Period");
            }
        }
       
        private TimeSpan _GameTaimer;
        public TimeSpan GameTaimer
        {
            get => _GameTaimer;
            set
            {
                _GameTaimer = value;
                OnPropertyChanged("GameTaimerString");
            }

        }

        
        public string GameTaimerString
        {
            get 
            {
                TimeSpan ts = GameTaimer;
                return string.Format("{0}:{1:ss}", Math.Floor(ts.TotalMinutes), ts);
            }
            set
            {
                List <int> MinutesAndSeconds = new List <int> (value.Split(":").Select(Int32.Parse).ToArray());
                GameTaimer = new TimeSpan (0, MinutesAndSeconds[0], MinutesAndSeconds[1]);
                OnPropertyChanged("GameTaimerString");
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
